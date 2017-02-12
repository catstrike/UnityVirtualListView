using UnityEngine;
using UnityEngine.UI;
using System;

namespace Controls
{
    public interface IListViewDataSource<ItemType>
    {
        event Action OnUpdate;

        int Count { get; }
        ItemType GetItem(int index);
        void Update();
    }

    public interface IListViewLayout
    {
        Vector2 CanvasSize { get; }
        Vector2 ViewportSize { get; set; }
        Vector2 ItemSpacing { get; set; }
        Vector2 ItemSize { get; set; }
        RectOffset Padding { get; set; }

        void GetVisibleRange(Vector2 scrollOffset, ref int firstIndex, ref int lastIndex);
        void GetItemPosition(int index, ref Vector3 position);
        void GetCanvasSize(ref Vector2 size);
    }

    public enum ListViewLayoutType
    {
        Cell,
        Horizontal,
        Vertical
    }

    public class ListView<ItemType, ItemDataType>: MonoBehaviour
        where ItemType : ListViewItem<ItemDataType>
        where ItemDataType : class
    {
        #region - UI Bindings
        [Header("Binding")]
        [SerializeField] ItemType itemTemplate;
        [SerializeField] ScrollRect scroll;
        [SerializeField] RectTransform content;

        [Header("Layout")]
        [SerializeField] ListViewLayoutType layoutType;
        [SerializeField] RectOffset Padding;
        [SerializeField] Vector2 ItemSpacing;
        [SerializeField] Vector2 ItemSize;
        #endregion

        #region - Properties
        public IListViewDataSource<ItemDataType> DataSource
        {
            get { return dataSource; }
            set { SetDataSource(value); }
        }

        public Vector2 ScrollPosition
        {
            get { return contentRectTransform.offsetMax; }
            set { contentRectTransform.offsetMax = value; }
        }
        #endregion

        #region - State
        RectTransform contentRectTransform;

        IListViewDataSource<ItemDataType> dataSource;
        IListViewLayout layout;
        ListViewItemsPool<ItemType> itemsPool;

        int lastVisibleFirstIndex;
        int lastVisibleLastIndex;
        #endregion

        #region - Lifecycle
        protected virtual void Awake()
        {
            itemsPool = new ListViewItemsPool<ItemType>(itemTemplate);
            itemsPool.SetupItem = SetupItem;

            scroll.onValueChanged.AddListener(OnScrollChangeHandler);

            content.anchorMax = new Vector2(0.0f, 1.0f);
            content.anchorMin = new Vector2(0.0f, 1.0f);
            content.pivot = new Vector2(0.0f, 1.0f);

            contentRectTransform = content.transform as RectTransform;

            CleanMocks();
        }
        #endregion

        #region - Public
        public void ScrollTo(int itemIndex)
        {
            var itemPosition = Vector3.zero;
            var contentSize = Vector2.zero;

            layout.GetItemPosition(itemIndex, ref itemPosition);
            layout.GetCanvasSize(ref contentSize);

            scroll.normalizedPosition = new Vector2(
                itemPosition.x / contentSize.x,
                itemPosition.y / contentSize.y
            );
        }
        #endregion

        #region - Overridable
        protected virtual void SetupItem(ItemType item, ItemDataType data)
        {

        }
        #endregion

        #region - Private
        IListViewLayout CreateLayout()
        {
            IListViewLayout result;
            switch (layoutType) {
                case ListViewLayoutType.Cell:
                    result = new ListViewCellLayout<ItemDataType>(dataSource);
                    break;
                case ListViewLayoutType.Horizontal:
                    result = new ListViewHorizontalLayout<ItemDataType>(dataSource);
                    break;
                case ListViewLayoutType.Vertical:
                default:
                    throw new NotImplementedException();
            }

            return result;
        }

        void UpdateLayout()
        {
            var scrollRectTransfomr = scroll.transform as RectTransform;
            Canvas.ForceUpdateCanvases();

            layout.ViewportSize = scrollRectTransfomr.rect.size;
            layout.ItemSpacing = ItemSpacing;
            layout.Padding = Padding;
        }

        void CleanMocks()
        {
            foreach (var item in content) {
                Destroy((item as Transform).gameObject);
            }
        }

        void SetDataSource(IListViewDataSource<ItemDataType> dataSource)
        {
            if (this.dataSource == dataSource) {
                return;
            }

            if (this.dataSource != null) {
                this.dataSource.OnUpdate -= RefreshData;
            }

            this.dataSource = dataSource;

            if (dataSource == null) {
                return;
            }

            dataSource.OnUpdate += RefreshData;

            layout = CreateLayout();
            layout.ItemSize = ItemSize;
            UpdateLayout();

            RefreshData();
            ScrollPosition = new Vector2(0, 0);
        }

        void RefreshData()
        {
            UpdateItems(forceUpdate: true);
        }

        void SetupItem(ItemType item)
        {
            var templateRectTransform = itemTemplate.GetComponent<RectTransform>();
            var itemRectTransform = item.GetComponent<RectTransform>();
            itemRectTransform.anchorMax = new Vector2(0.0f, 1.0f);
            itemRectTransform.anchorMin = new Vector2(0.0f, 1.0f);

            if (ItemSize.x == 0 && ItemSize.y == 0) {
                itemRectTransform.sizeDelta = templateRectTransform.sizeDelta;
            } else {
                itemRectTransform.sizeDelta = ItemSize;
            }

            itemRectTransform.sizeDelta = ItemSize;
        }

        void UpdateItems(bool forceUpdate)
        {
            var canvasSize = Vector2.zero;
            var itemPosition = Vector3.zero;
            int startVisibleIndex = 0;
            int endVisibleIndex = 0;

            var scrollOffset = new Vector2(
                content.offsetMin.x,
                content.offsetMax.y
            );

            layout.GetCanvasSize(ref canvasSize);
            layout.GetVisibleRange(scrollOffset, ref startVisibleIndex, ref endVisibleIndex);

            if (!forceUpdate && 
                startVisibleIndex == lastVisibleFirstIndex &&
                endVisibleIndex == lastVisibleLastIndex) {
                return;
            }

            content.sizeDelta = canvasSize;

            var itemRectTransform = itemTemplate.GetComponent<RectTransform>();

            var itemOffset = new Vector3(
                ItemSize.x * itemRectTransform.pivot.x,
                ItemSize.y * (1 - itemRectTransform.pivot.y)
            );

            itemsPool.BeginBatch();

            for (var i = startVisibleIndex; i <= endVisibleIndex; i += 1) {
                var itemData = dataSource.GetItem(i);

                var item = itemsPool.GetItem();

                var itemTransform = item.transform as RectTransform;
                itemTransform.SetParent(content, worldPositionStays: false);

                layout.GetItemPosition(i, ref itemPosition);
                itemPosition.y = -(itemPosition.y + itemOffset.y);
                itemPosition.x += itemOffset.x;
                itemTransform.anchoredPosition = itemPosition;

                item.ResetItem();
                item.SetData(itemData);
                SetupItem(item, itemData);
            }

            itemsPool.EndBatch();

            lastVisibleFirstIndex = startVisibleIndex;
            lastVisibleLastIndex = endVisibleIndex;
        }

        void OnScrollChangeHandler(Vector2 scrollPosition)
        {
            if (layout == null) {
                return;
            }

            UpdateItems(forceUpdate: false);
        }
        #endregion
    }
}
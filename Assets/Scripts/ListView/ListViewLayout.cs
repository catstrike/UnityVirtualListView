using UnityEngine;

namespace Controls
{
    public abstract class ListViewLayout<ItemDataType>: IListViewLayout
    {
        #region - Properties
        public Vector2 CanvasSize
        {
            get { return canvasSize; }
        }
        public Vector2 ViewportSize {
            get { return viewportSize; }
            set {
                viewportSize = value;
                CalculateParameters();
            }
        }

        public Vector2 ItemSpacing {
            get { return itemSpace; }
            set { itemSpace = value; }
        }

        public RectOffset Padding {
            get { return padding; }
            set { padding = value; }
        }

        public Vector2 ItemSize { get; set; }
        #endregion

        #region - State
        protected RectOffset padding;
        protected Vector2 canvasSize;
        protected Vector2 viewportSize;
        protected Vector2 itemSpace;
        
        protected IListViewDataSource<ItemDataType> dataSource;
        #endregion

        #region - Lifecycle
        public ListViewLayout(IListViewDataSource<ItemDataType> dataSource)
        {
            this.dataSource = dataSource;
        }
        #endregion

        #region - Public
        public abstract void GetVisibleRange(Vector2 scrollOffset, ref int firstIndex, ref int lastIndex);
        public abstract void GetItemPosition(int index, ref Vector3 position);
        public abstract void GetCanvasSize(ref Vector2 size);
        #endregion

        #region - Private
        protected abstract void CalculateParameters();
        #endregion
    }
}
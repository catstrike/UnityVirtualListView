using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace Controls
{
    public class ListViewVerticalAdaptableLayout<ItemType, ItemDataType>: ListViewLayout<ItemDataType>
    {
        #region - State
        readonly ListViewItem<ItemDataType> itemTemplate;
        readonly List<float> itemsPositions = new List<float>();
        #endregion

        #region - Lifecycle
        public ListViewVerticalAdaptableLayout(IListViewDataSource<ItemDataType> dataSource, ListViewItem<ItemDataType> itemTemplate)
            : base(dataSource)
        {
            this.itemTemplate = itemTemplate;

            dataSource.OnUpdate += CalculateParameters;
        }
        #endregion

        #region - Public
        public override void GetCanvasSize(ref Vector2 size)
        {
            size.y = itemsPositions[dataSource.Count];
            size.x = canvasSize.x;
        }

        public override void GetItemPosition(int index, ref Vector3 position)
        {
            position.x = padding.left;
            position.y = itemsPositions[index];
        }

        public override void GetVisibleRange(Vector2 scrollOffset, ref int firstIndex, ref int lastIndex)
        {
            var start = scrollOffset.y;
            var end = start + viewportSize.y;

            var dataCount = dataSource.Count;
            if (dataCount == 0) {
                firstIndex = -1;
                lastIndex = -2;

                return;
            }

            for (int i = 0; i < dataCount; i++) {
                if (itemsPositions[i] > start) {
                    break;
                }

                firstIndex = i;
            }

            for (int i = firstIndex; i < dataCount; i++) {
                lastIndex = i;

                if (itemsPositions[i] > end) {
                    break;
                }
            }
        }
        #endregion

        #region - Private
        protected override void CalculateParameters()
        {
            canvasSize = new Vector2(viewportSize.x, 0);

            itemsPositions.Clear();

            var dataCount = dataSource.Count;
            var itemSpaceY = itemSpace.y;
            var totalYSpace = (float)padding.top;

            itemsPositions.Add(totalYSpace);

            if (dataCount == 0) {
                return;
            }

            var itemLayoutElement = itemTemplate.GetComponent<ILayoutElement>();

            itemTemplate.gameObject.SetActive(true);

            for (int i = 0; i < dataCount - 1; i++) {
                var data = dataSource.GetItem(i);

                totalYSpace += GetPreferredHeight(itemLayoutElement, data) + itemSpaceY;

                itemsPositions.Add(totalYSpace);
            }

            var lastData = dataSource.GetItem(dataCount - 1);
            totalYSpace += GetPreferredHeight(itemLayoutElement, lastData) + padding.bottom;

            itemsPositions.Add(totalYSpace);

            itemTemplate.gameObject.SetActive(false);
        }

        float GetPreferredHeight(ILayoutElement itemLayoutElement, ItemDataType data)
        {
            itemTemplate.SetData(data);
            itemLayoutElement.CalculateLayoutInputVertical();

            return itemLayoutElement.preferredHeight;
        }
        #endregion
    }
}
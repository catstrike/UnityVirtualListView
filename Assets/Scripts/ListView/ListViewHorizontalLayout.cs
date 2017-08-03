using System;
using UnityEngine;

namespace Controls
{
    public class ListViewHorizontalLayout<ItemDataType>: ListViewLayout<ItemDataType>
    {
        #region - State
        int itemWidth;
        #endregion

        #region - Lifecycle
        public ListViewHorizontalLayout(IListViewDataSource<ItemDataType> dataSource)
            : base(dataSource)
        {
        }
        #endregion

        #region - Public
        public override void GetCanvasSize(ref Vector2 size)
        {
            var totalItems = dataSource.Count;

            var totalXSpace = (totalItems > 1) ? (totalItems - 1) * itemSpace.x : 0;

            size.y = canvasSize.y;
            size.x = totalItems * itemWidth + totalXSpace + padding.left + padding.right;
        }

        public override void GetItemPosition(int index, ref Vector3 position)
        {
            position.x = index * (itemWidth + itemSpace.x) + padding.left;
            position.y = padding.top;
        }

        public override void GetVisibleRange(Vector2 scrollOffset, ref int firstIndex, ref int lastIndex)
        {
            var offset = scrollOffset.x;
            var itemAndSpaceWidth = (itemWidth + itemSpace.x);

            firstIndex = Math.Max(0, (int)(-offset / itemAndSpaceWidth));
            lastIndex = Math.Min(
                dataSource.Count,
                Mathf.CeilToInt((ViewportSize.x - offset) / itemAndSpaceWidth)
            ) - 1;
        }
        #endregion

        #region - Private
        protected override void CalculateParameters()
        {
            canvasSize = new Vector2(0, viewportSize.y);
            itemWidth = (int)ItemSize.x;
        }
        #endregion
    }
}
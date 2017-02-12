using UnityEngine;
using System;

namespace Controls
{
    public class ListViewCellLayout<ItemDataType>: ListViewLayout<ItemDataType>
    {
        #region - State
        protected int itemsPerRow;
        protected int cellWidth;
        protected int cellHeight;
        #endregion

        #region - Lifecycle
        public ListViewCellLayout(IListViewDataSource<ItemDataType> dataSource) 
            : base(dataSource)
        {
        }
        #endregion

        #region - Public
        public override void GetItemPosition(int index, ref Vector3 position)
        {
            var row = index / itemsPerRow;
            var column = index % itemsPerRow;

            position.x = column * (cellWidth + itemSpace.x) + padding.left;
            position.y = row * (cellHeight + itemSpace.y) + padding.top;
        }

        public override void GetVisibleRange(Vector2 scrollOffset, ref int firstIndex, ref int lastIndex)
        {
            var offset = scrollOffset.y;
            var firstRow = (int)(offset / (cellHeight + itemSpace.y));
            var lastRow = Mathf.CeilToInt((offset + ViewportSize.y) / (cellHeight + itemSpace.y));

            firstIndex = Math.Max(0, firstRow * itemsPerRow);
            lastIndex = Math.Min(dataSource.Count, lastRow * itemsPerRow) - 1;
        }

        public override void GetCanvasSize(ref Vector2 size)
        {
            var totalRows = Mathf.CeilToInt((float)dataSource.Count / itemsPerRow);

            var totalYSpace = (totalRows > 1) ? (totalRows - 1) * itemSpace.y : 0;

            size.x = canvasSize.x;
            size.y = totalRows * cellHeight + totalYSpace + padding.top + padding.bottom;
        }
        #endregion

        #region - Private
        protected override void CalculateParameters()
        {
            canvasSize = new Vector2(viewportSize.x, 0);
            cellWidth = (int)ItemSize.x;
            cellHeight = (int)ItemSize.y;

            itemsPerRow = (int)(canvasSize.x / cellWidth);
        }
        #endregion
    }
}
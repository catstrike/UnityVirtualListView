using System.Collections.Generic;
using UnityEngine;

namespace Controls
{
    public class ListViewItemsPool<ItemType>
        where ItemType : MonoBehaviour
    {
        #region - Properties
        public System.Action<ItemType> SetupItem { get; set; }
        #endregion

        #region - State
        readonly ItemType template;
        readonly Stack<ItemType> itemsStack = new Stack<ItemType>();
        readonly List<ItemType> activeItems = new List<ItemType>();

        bool batchMode;
        #endregion

        #region - Lifecycle
        public ListViewItemsPool(ItemType template)
        {
            this.template = template;
        }
        #endregion

        #region - Public
        public ItemType GetItem()
        {
            var item = GetItemInternal();

            if (!batchMode) {
                item.gameObject.SetActive(true);
            }

            return item;
        }

        public void Recycle(ItemType item)
        {
            if (batchMode) {
                return;
            }

            activeItems.Remove(item);
            RecycleInternal(item);
        }

        public void RecycleAll()
        {
            if (batchMode) {
                return;
            }

            foreach (var item in activeItems) {
                RecycleInternal(item);
            }

            activeItems.Clear();
        }

        public void BeginBatch()
        {
            if (batchMode) {
                return;
            }

            batchMode = true;

            var activeItemsCount = activeItems.Count;

            for (var i = activeItemsCount - 1; i >= 0; i -= 1) {
                var item = activeItems[i];
                itemsStack.Push(item);
            }

            activeItems.Clear();
        }

        public void EndBatch()
        {
            var activeItemsCount = activeItems.Count;

            for (var i = 0; i < activeItemsCount; i += 1) {
                var item = activeItems[i];
                item.gameObject.SetActive(true);
            }

            foreach (var item in itemsStack) {
                item.gameObject.SetActive(false);
            }

            batchMode = false;
        }

        #endregion

        #region - Private
        void RecycleInternal(ItemType item)
        {
            itemsStack.Push(item);
            item.gameObject.SetActive(false);
        }

        ItemType GetItemInternal()
        {
            ItemType item;

            if (itemsStack.Count == 0) {
                item = Object.Instantiate(template);
                if (SetupItem != null) {
                    SetupItem(item);
                }
            } else {
                item = itemsStack.Pop();
            }

            activeItems.Add(item);

            return item;
        }
        #endregion
    }
}
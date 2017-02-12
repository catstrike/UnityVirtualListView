using UnityEngine;

namespace Controls
{
    public abstract class ListViewItem<DataType>: MonoBehaviour
    {
        #region - Public
        public abstract void SetData(DataType data);
        public virtual void ResetItem()
        {
        }
        #endregion
    }
}
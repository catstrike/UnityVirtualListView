using Controls;
using UnityEngine;
using UnityEngine.UI;

namespace Examples.Simple
{
    public class StoreView: ListView<StoreItem, StoreItemData>
    {
        #region - UI Bindings
        [Header("Store Selected Item")]
        [SerializeField] Text selectedItemTitle;
        [SerializeField] Text selectedItemPrice;
        [SerializeField] Text selectedItemDescription;
        #endregion

        #region - Lifecycle
        void Start()
        {
            DataSource = new StoreDataSource();

            SetSelectedItem(new StoreItemData());
        }
        #endregion

        #region - Private
        protected override void SetupItem(StoreItem item, StoreItemData data)
        {
            base.SetupItem(item, data);

            item.OnItemClick += () => {
                SetSelectedItem(data);
            };

            item.OnBuyClick += () => {
                data.Purchased = true;
                DataSource.Update();
            };
        }

        void SetSelectedItem(StoreItemData data)
        {
            selectedItemTitle.text = data.Title;
            selectedItemPrice.text = data.Price.ToString();
            selectedItemDescription.text = data.Descriptions;
        }
        #endregion
    }
}

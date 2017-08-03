using System;
using Controls;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Examples.Simple
{
    public class StoreItem: ListViewItem<StoreItemData>
    {
        #region - UI Bindings
        [SerializeField] Text title;
        [SerializeField] Text buttonText;
        [SerializeField] Button buyButton;
        #endregion

        #region - Events
		public event UnityAction<StoreItemData> OnBuyClick {
			add { buyButton.onClick.AddListener(() => value(data)); }
			remove { throw new NotImplementedException(); }
        }

		public event UnityAction<StoreItemData> OnItemClick {
			add { thisButton.onClick.AddListener(() => value(data)); }
			remove { throw new NotImplementedException(); }
        }
        #endregion

        #region - State
        Button thisButton;
		StoreItemData data;
        #endregion

        #region - Lifecycle
        void Awake()
        {
            thisButton = GetComponent<Button>();
        }
        #endregion

        #region - Public
        public override void SetData(StoreItemData data)
        {
			this.data = data;

            buyButton.gameObject.SetActive(!data.Purchased);

            title.text = data.Title;
            buttonText.text = data.Price + " EUR";
        }
        #endregion
    }
}

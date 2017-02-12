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
        public event UnityAction OnBuyClick {
            add { buyButton.onClick.AddListener(value); }
            remove { buyButton.onClick.RemoveListener(value); }
        }

        public event UnityAction OnItemClick {
            add { thisButton.onClick.AddListener(value); }
            remove { thisButton.onClick.RemoveListener(value); }
        }
        #endregion

        #region - State
        Button thisButton;
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
            buyButton.gameObject.SetActive(!data.Purchased);

            title.text = data.Title;
            buttonText.text = data.Price + " EUR";
        }

        public override void ResetItem()
        {
            buyButton.onClick.RemoveAllListeners();
            thisButton.onClick.RemoveAllListeners();
        }
        #endregion
    }
}

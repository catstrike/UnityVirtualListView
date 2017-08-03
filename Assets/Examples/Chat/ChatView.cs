using Controls;
using UnityEngine;
using UnityEngine.UI;

namespace Examples.Chat
{
    public class ChatView: ListView<ChatItem, ChatItemData>
    {
        #region - UI Bindings
        [Header("Chat buttons")]
        [SerializeField] Button addTopMessagesButton;
		[SerializeField] Button addBottomMessagesButton;
        #endregion

        #region - Lifecycle
		protected override void Awake()
		{
			base.Awake();

			itemTemplate.gameObject.SetActive(false);
		}

        void Start()
        {
			var chatDataSource = new ChatDataSource();

			DataSource = chatDataSource;

			addTopMessagesButton.onClick.AddListener(() => {
				chatDataSource.PrependRandomMessages();
				chatDataSource.Update();
			});

			addBottomMessagesButton.onClick.AddListener(() => {
				chatDataSource.AppendRandomMessages();
				chatDataSource.Update();
			});
        }
        #endregion
    }
}

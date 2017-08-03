using System;
using Controls;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Examples.Chat
{
    public class ChatItem: ListViewItem<ChatItemData>
    {
        #region - UI Bindings
        [SerializeField] Text userIdText;
        [SerializeField] Text messageText;
        #endregion

        #region - Public
        public override void SetData(ChatItemData data)
        {
			userIdText.text = data.UserID.ToString();
			messageText.text = data.MessageWithMessageID;
        }
        #endregion
    }
}

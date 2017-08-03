using System;
using System.Collections.Generic;
using System.Linq;
using Controls;

namespace Examples.Chat
{
    public class ChatDataSource: IListViewDataSource<ChatItemData>
    {
		#region - Constant
		const string chars = "ABCDEFGHI JKLMNOPQRS TUVWXYZ abcdefghij klmnopqrstuvwxyz 0123456789";
		#endregion

        #region - Properties
        public int Count {
            get { return items.Count; }
        }
        #endregion

        #region - Events
        public event Action OnUpdate;
        #endregion

        #region - State
		List<ChatItemData> items = new List<ChatItemData>();
        #endregion

        #region - Public
        public ChatItemData GetItem(int index)
        {
            return items[index];
        }

        public void Update()
        {
            if (OnUpdate != null) {
                OnUpdate();
            }
        }

		public void AppendRandomMessages()
		{
			var messages = GetRandomMessages(10);

			items.AddRange(messages);

			CreateMessageWithIndex();
		}

		public void PrependRandomMessages()
		{
			var messages = GetRandomMessages(10);

			messages.AddRange(items);

			items = messages;

			CreateMessageWithIndex();
		}
        #endregion

		#region - Private
		void CreateMessageWithIndex()
		{
			for (int i = 0; i < items.Count; i++) {
				var messageData = items[i];

				messageData.MessageWithMessageID = string.Format(
					"Message index {0}: {1}",
					i, messageData.Message
				);
			}
		}

		List<ChatItemData> GetRandomMessages(int count)
		{
			var random = new Random();
			var result = new List<ChatItemData>();

			for (int i = 0; i < count; i++) {
				var randomUserID = random.Next(0, 10);

				var randomMessageLength = random.Next(1, 200);

				var randomMessage = new string(
					Enumerable.Repeat(chars, randomMessageLength)
					.Select(s => s[random.Next(s.Length)])
					.ToArray()
				);

				result.Add(new ChatItemData {
					UserID = randomUserID,
					Message = randomMessage
				});
			}

			return result;
		}
		#endregion
    }
}

using System;

namespace Messenger.Core.Entities
{
    public class OutMessage : MessageBase
    {
        public bool Sent { get; set; }

        public OutMessage(string messageText)
        {
            MessageText = messageText;
            CreatedAt = DateTime.Now;
            Sent = false;
        }
    }
}

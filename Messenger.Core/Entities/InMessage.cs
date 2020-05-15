using System;

namespace Messenger.Core.Entities
{
    public class InMessage : MessageBase
    {
        public string IpAdress { get; set; }
        public DateTime RecivedAt { get; set; }

        public InMessage(string messageText, string ipAdress, DateTime createdAt)
        {
            MessageText = messageText;
            IpAdress = ipAdress;
            CreatedAt = createdAt;
            RecivedAt = DateTime.Now;
        }

    }
}

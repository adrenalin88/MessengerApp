using System;

namespace Messenger.Core.Entities
{
    public abstract class MessageBase : EntityBase
    {
        public string MessageText { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

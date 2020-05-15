using Messenger.Core.Entities;
using System.Threading.Tasks;

namespace Messenger.BLL.Interfaces
{
    public interface IMessageSender
    {
        Task<MessageSendResult> SendMessageAsync(string url, MessageBase message);
    }
}

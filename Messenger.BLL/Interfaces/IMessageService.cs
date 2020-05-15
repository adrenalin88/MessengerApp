using Messenger.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Messenger.BLL.Interfaces
{
    public interface IMessageService
    {
        void AddMessage(InMessage message);
        Task AddMessageAsync(InMessage message);
        IEnumerable<InMessage> GetMessages();
        Task<IEnumerable<InMessage>> GetMessagesAsync();
    }
}

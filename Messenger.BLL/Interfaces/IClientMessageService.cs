using Messenger.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Messenger.BLL.Interfaces
{
    public interface IClientMessageService
    {
        void SaveMessage(OutMessage message);
        Task SaveMessageAsync(OutMessage message);
        void MarkAsSent(OutMessage message);
        Task MarkAsSentAsync(OutMessage message);
        IEnumerable<OutMessage> GetUnsent();
        Task<IEnumerable<OutMessage>> GetUnsentAsync();
    }
}

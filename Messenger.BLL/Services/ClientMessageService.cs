using Messenger.BLL.Interfaces;
using Messenger.Core.Entities;
using Messenger.DAL.EF;
using Messenger.DAL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Messenger.BLL.Services
{
    public class ClientMessageService : IClientMessageService
    {
        private IRepository<MessageContext<OutMessage>, OutMessage> MessageRepository { get; set; }

        public ClientMessageService(IRepository<MessageContext<OutMessage>, OutMessage> messageRepository)
        {
            MessageRepository = messageRepository;
        }
        public void SaveMessage(OutMessage message)
        {
            MessageRepository.Create(message);
            MessageRepository.Save();
        }

        public async Task SaveMessageAsync(OutMessage message)
        {
            MessageRepository.Create(message);
            await MessageRepository.SaveAsync();
        }

        public void MarkAsSent(OutMessage message)
        {
            MessageRepository.Update(message);
            MessageRepository.Save();
        }

        public async Task MarkAsSentAsync(OutMessage message)
        {
            MessageRepository.Update(message);
            await MessageRepository.SaveAsync();
        }

        public IEnumerable<OutMessage> GetUnsent() => MessageRepository.Find(m => !m.Sent);

        public Task<IEnumerable<OutMessage>> GetUnsentAsync() => MessageRepository.FindAsync(m => !m.Sent);
    }
}

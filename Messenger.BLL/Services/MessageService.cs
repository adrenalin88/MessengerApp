using Messenger.BLL.Interfaces;
using Messenger.Core.Entities;
using Messenger.DAL.EF;
using Messenger.DAL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Messenger.BLL.Services
{
    public class MessageService : IMessageService
    {
        private IRepository<MessageContext<InMessage>, InMessage> MessageRepository { get; set; }

        public MessageService(IRepository<MessageContext<InMessage>, InMessage> messageRepository)
        {
            MessageRepository = messageRepository;
        }

        public void AddMessage(InMessage message)
        {
            MessageRepository.Create(message);
            MessageRepository.Save();
        }

        public async Task AddMessageAsync(InMessage message)
        {
            MessageRepository.Create(message);
            await MessageRepository.SaveAsync();
        }

        public IEnumerable<InMessage> GetMessages() => MessageRepository.GetAll();


        public async Task<IEnumerable<InMessage>> GetMessagesAsync() => await MessageRepository.GetAllAsync();
    }
}

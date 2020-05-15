using Messenger.BLL.Interfaces;
using Messenger.Core.Entities;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Messenger.BLL.Services
{
    public class MessageSender : IMessageSender
    {
        public async Task<MessageSendResult> SendMessageAsync(string url, MessageBase message)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.PostAsJsonAsync(url, message);
                    return MessageSendResult.FromResponseStatusCode(response.StatusCode);
                }
                catch (Exception xcp)
                {
                    return MessageSendResult.FromException(xcp);
                }
            }
        }
    }
}

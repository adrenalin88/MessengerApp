using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using log4net;
using Messenger.BLL.Interfaces;
using Messenger.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MessengerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InMessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(InMessagesController));

        public InMessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet]
        public async Task<IEnumerable<InMessage>> Get()
        {
            _logger.Info("Получен запрос списка всех сообщений");
            return await _messageService.GetMessagesAsync();
        }

        [HttpPost]
        public async Task<ActionResult<InMessage>> PostMessage(InMessage message)
        {
            if (message == null || string.IsNullOrEmpty(message.MessageText))
                return BadRequest();

            message.RecivedAt = DateTime.Now;
            message.IpAdress = ControllerContext.HttpContext.Connection.RemoteIpAddress.ToString();
            _logger.Info($"Сообщение '{message.MessageText}' получено от {message.IpAdress} в {message.RecivedAt}");
            await _messageService.AddMessageAsync(message);
            return CreatedAtAction("PostMessage", new { id = message.Id }, message);
        }
    }
}
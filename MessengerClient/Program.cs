using log4net;
using log4net.Config;
using Messenger.BLL;
using Messenger.BLL.Interfaces;
using Messenger.BLL.Services;
using Messenger.Core.Entities;
using Messenger.DAL.EF;
using Messenger.DAL.Interfaces;
using Messenger.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace MessengerClient
{
    class Program
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static IServiceProvider _serviceProvider;
        static void Main(string[] args)
        {            
            ConfigureLogging();
            RegisterServices();

            while (true)
            {
                Console.WriteLine("Введите текст сообщения для отправки на сервер или '/выход' для завершения работы программы:");
                var message = Console.ReadLine();
                if (message.ToLower() == "/выход")
                    break;
                ProceedMessageAsync(message);
            }

            DisposeServices();
        }

        private static void ConfigureLogging()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }
        private static void RegisterServices()
        {
            var collection = new ServiceCollection();
            collection.AddDbContext<MessageContext<OutMessage>>(options =>
                options.UseSqlServer(ConfigurationManager.ConnectionStrings["ClientMessageDB"].ToString()), ServiceLifetime.Transient);
            collection.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            collection.AddScoped<IClientMessageService, ClientMessageService>();
            collection.AddScoped<IMessageSender, MessageSender>();
            _serviceProvider = collection.BuildServiceProvider();
        }
        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }

        // Обработка новых сообщений с консоли.
        static async Task ProceedMessageAsync(string messageText)
        {
            _log.Info($"Получено сообщение '{messageText}'");
            var message = new OutMessage(messageText);
            await SaveMessageAsync(message);
            await TrySendMessageAsync(message);
        }

        // Сохранение сообщений в локальную БД.
        static async Task SaveMessageAsync(OutMessage message)
        {
            var messageService = _serviceProvider.GetService<IClientMessageService>();
            await messageService.SaveMessageAsync(message);
            _log.Info($"Сообщение '{message.MessageText}' сохранено в локальную БД. Присвоен идентификатор: {message.Id}");
        }

        // Отправка сообщений на сервер. Если сообщение отправлено не было - оно будет обработано вместе с остальными неотправленными сообщениями.
        static async Task<bool> TrySendMessageAsync(OutMessage message)
        {
            var messageSender = _serviceProvider.GetService<IMessageSender>();

            var result = MessageSendResult.BeforeSend();
            while (!result.Sucsess)
            {
                result = await messageSender.SendMessageAsync("https://localhost:44370/api/InMessages", new InMessage(message.MessageText, message.CreatedAt));
                if (result.Sucsess)
                {
                    message.Sent = true;
                    var messageService = _serviceProvider.GetService<IClientMessageService>();
                    await messageService.MarkAsSentAsync(message);
                    _log.Info($"Сообщение {message.Id}('{message.MessageText}') отправлено на сервер");
                }
                else
                {
                    if (result.Exception != null)
                        _log.Error("Ошибка при отправке сообщения на сервер", result.Exception);
                    else
                        _log.Warn($"При отправке сообщения получен ответ {result.ResponseCode}");
                }
                Thread.Sleep(500000);
            }
            return true;
        }
    }
}

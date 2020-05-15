using log4net;
using log4net.Config;
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
    }
}

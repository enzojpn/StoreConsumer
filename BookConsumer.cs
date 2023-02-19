using BookStoreApi.Models;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace StoreConsumer
{
    internal class BookConsumer : IHostedService
    {
        static IQueueClient? queueClient;
        private readonly IConfiguration _configuration;
        public BookConsumer(IConfiguration configuration)
        {
            _configuration = configuration;
            var serviceBusConnection = _configuration.GetSection("AzureServiceBus").Value;
            queueClient = new QueueClient(serviceBusConnection, "books");
        }

        public Task StartAsync(CancellationToken token)
        {
            Console.WriteLine(">>>>>START CONSUMER - QUEUE<<<<<<<<<");
            ProcessMessageHandler();
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken token)
        {
            Console.WriteLine(">>>>>STOPPING CONSUMER - QUEUE<<<<<<<<<");

            if (queueClient != null)
                await queueClient.CloseAsync();

            await Task.CompletedTask;
        }

        public void ProcessMessageHandler()
        {

            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            if (queueClient != null)
                queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        public async Task ProcessMessagesAsync(Message? message, CancellationToken token)
        {
            Console.WriteLine(">>>>Processing Message - Queue <<<<");
            Console.WriteLine($">>>>DateTime - {DateTime.Now}<<<<");

            if (message != null)
            { 
            Console.WriteLine($">>>>receive message - {message.SystemProperties.SequenceNumber} body{Encoding.UTF8.GetString(message.Body)}<<<<");
                Book _book = JsonSerializer.Deserialize<Book>(message.Body);
            }

            if (queueClient != null && message !=null)
            await queueClient.CompleteAsync(message.SystemProperties.LockToken);

        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encounter a exception{exceptionReceivedEventArgs.Exception}");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine($"Endpoint: {context.Endpoint} | Entity Path: {context.EntityPath} | Executing Action: {context.Action}");

            return Task.CompletedTask;
        }


    }
}

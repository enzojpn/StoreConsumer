// See https://aka.ms/new-console-template for more information


using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StoreConsumer;

Console.WriteLine("==STORE CONSUMER AZURE SERVICE BUS==");

var serviceCollection = new ServiceCollection();

IConfiguration configuration;
 
configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
    .AddJsonFile("appsettings.json")
    .Build();

serviceCollection.AddSingleton<IConfiguration>(configuration);
var host = new HostBuilder()
          .ConfigureHostConfiguration(configHost => {
          })
          .ConfigureServices((hostContext, services) => {
              services.AddHostedService<BookConsumer>();
              services.AddSingleton<IConfiguration>(configuration);
          })
         .UseConsoleLifetime()
         .Build();
 
host.Run();




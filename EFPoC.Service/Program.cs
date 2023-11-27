using EFPoC.DAL;
using EFPoC.DAL.Models;
using EFPoC.Service;
using EFPoC.SL;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.Console;

var builder = Host.CreateDefaultBuilder().UseWindowsService().ConfigureServices((_, services) => {
    LoggerProviderOptions.RegisterProviderOptions<ConsoleLoggerOptions, ConsoleLoggerOptions>(services);

    services.AddDbContext<MyDbContext>();

    services.AddDAL();

    services.AddTransient<UserService>();

    services.AddHostedService<MyService>();
});

await builder.RunConsoleAsync();
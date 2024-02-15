using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReenbitTestTaskAzureFunction.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
        services.AddSingleton<EmailService>())
    .Build();

host.Run();

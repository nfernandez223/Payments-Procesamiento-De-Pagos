using Application.Interfaces;
using Domain.Entities;
using InfraEstructure.Persistence;
using InfraEstructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkerProcesamientoDePagos;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString), ServiceLifetime.Singleton);

        var configuration = hostContext.Configuration;

        var rabbitMQConfig = configuration.GetSection("RabbitMQConfig").Get<RabbitMQConfig>();

        services.AddSingleton<IMessageService>(provider =>
        new MessageService(rabbitMQConfig.HostName, rabbitMQConfig.QueueNameConsumer, rabbitMQConfig.QueueNamePublish));

        services.AddSingleton<IContextMgmt, ContextMgmt>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();

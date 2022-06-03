using WorkerService1;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker1>();
        services.AddHostedService<Worker2>();
    })
    .Build();

await host.RunAsync();

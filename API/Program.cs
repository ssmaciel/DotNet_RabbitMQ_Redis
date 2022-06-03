using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton((sp) =>
{
    return new RabbitMQ.Client.ConnectionFactory()
    {
        Uri = new Uri(builder.Configuration.GetSection("DevWeek:RabbitMQ:ConnectionString").Get<string>())
    };
});

builder.Services.AddSingleton((sp) =>
{
    return sp.GetRequiredService<ConnectionFactory>().CreateConnection();
});

builder.Services.AddTransient((sp) =>
{
    return sp.GetRequiredService<IConnection>().CreateModel();
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

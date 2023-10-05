using Neoledge.AI.Orchestrator.Workers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient("HttpClientASP", (serviceProvider, client) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var baseUrl = configuration.GetValue<string>("MyHttpClient:BaseUrlASP");
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddHttpClient("HttpClientFlask", (serviceProvider, client) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var baseUrl = configuration.GetValue<string>("MyHttpClient:BaseUrlFlask");
    client.BaseAddress = new Uri(baseUrl);
});
builder.Services.AddHostedService<Orchestrator>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StockService.Data;
using StockService.EventProcessor;
using StockService.ItemServiceHttpClient;
using StockService.Models;
using StockService.RabbitMqClient;
using StockService.Repository;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ProductConnection");

builder.Services.AddDbContext<ProdutoContext>(opts =>
    opts.UseLazyLoadingProxies().UseNpgsql(connectionString));


builder.Services.AddHttpClient<IOrderServiceHttpClient, OrderServiceHttpClient>();
builder.Services.AddSingleton<IProcessaEvento, ProcessaEvento>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddHostedService<RabbitMqSubscriber>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "StockService", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () =>
{
    return "Executando";
})
.WithName("/")
.WithOpenApi();

app.UseHttpsRedirection();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ProdutoContext>();
    db.Database.Migrate();
}


app.Run();

public partial class Program;
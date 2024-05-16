using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Web.Watermark.BackgroundServices;
using RabbitMQ.Web.Watermark.Models;
using RabbitMQ.Web.Watermark.Services;

var configurationBuilder = new ConfigurationBuilder();

IConfiguration configuration = configurationBuilder.Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase(databaseName: "productDb");
});

builder.Services.AddSingleton(sp => new ConnectionFactory() { Uri = new Uri("amqps://tpxqyfmf:iNMQ1XlGeXnXelhoQPmath3dDL72moJ8@woodpecker.rmq.cloudamqp.com/tpxqyfmf") });

builder.Services.AddSingleton<RabbitMQClientService>();
builder.Services.AddSingleton<RabbitMQPublisher>();

builder.Services.AddHostedService<ImageWatermarkProcessBackgroundService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

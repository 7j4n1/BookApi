
using System.Globalization;
using System.Text.Json.Serialization;
using BookApi.Cache;
using BookApi.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BookApi;

public class Program()
{
    public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<LibraryDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddScoped<ICacheService, CacheService>();
        builder.Services.AddScoped<IAuthorService, AuthorService>();
        builder.Services.AddScoped<IAuthorHelper, AuthorService>();
        builder.Services.AddScoped<IBookService, BookService>();
        builder.Services.AddScoped<IPublisherHelper, PublisherService>();
        builder.Services.AddScoped<IPublisherService, PublisherService>();
        builder.Services.AddScoped<IMessageProducer, RabbitMQProducer>();
        builder.Services.AddSingleton<IMessageStorageService, MessageStorageService>();

        // Register the background service
        builder.Services.AddHostedService<MessageQueueConsumer>();

        builder.Services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("fr-FR"),
                // Add more cultures here
            };

            options.DefaultRequestCulture = new RequestCulture("en-US");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });




        

        // Add the necessary using directive for the AddNewtonsoftJson method
        builder.Services.AddControllers().AddJsonOptions(Options => {
            Options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        
        app.UseRequestLocalization();

        app.MapControllers();

        app.Run();
    }
}

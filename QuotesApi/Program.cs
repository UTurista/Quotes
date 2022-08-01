using Serilog;
using Serilog.Events;
using Quotes.Core.Db;
using Microsoft.EntityFrameworkCore;
using Quotes.Core.Services;
using Quotes.Api.Services;
using Quotes.Core.Db.Repositories;

namespace Quotes
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
             .WriteTo.Console()
             .CreateBootstrapLogger();

            try
            {
                var builder = WebApplication.CreateBuilder(args);
                builder.Host.UseSerilog((_, services, configuration) => ConfigureLogging(services, configuration));

                // Add services to the container.
#if !DEBUG
                builder.Services.AddApplicationInsightsTelemetry(x =>
                {
                    x.ConnectionString = builder.Configuration.GetConnectionString("ApplicationInsights");
                });
#endif
                builder.Services.AddDbContext<QuoteDbContext>(x => x.UseSqlite(@"Data Source=QuotesContext.db;"));
                builder.Services.AddScoped<IQuoteDbUnitOfWork, QuoteDbUnitOfWork>();
                builder.Services.AddScoped<IPersonRepository, PersonRepository>();
                builder.Services.AddScoped<IQuoteRepository, QuoteRepository>();
                builder.Services.AddScoped<IPersonService, PersonService>();
                builder.Services.AddScoped<IQuoteService, QuoteService>();
                builder.Services.AddSingleton<ITransformService, TransformService>();
                builder.Services.AddSingleton<IIdService, IdService>();

                builder.Services.AddControllers();
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
                app.MapControllers();

   
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "An unhandled exception occured during bootstrapping");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void ConfigureLogging(IServiceProvider services, LoggerConfiguration loggerConfiguration)
        {
#if DEBUG
            loggerConfiguration.MinimumLevel.Verbose();
#else
            loggerConfiguration.MinimumLevel.Debug();
#endif

            loggerConfiguration.MinimumLevel.Override("Microsoft", LogEventLevel.Information);

            loggerConfiguration.Enrich.FromLogContext();
            loggerConfiguration.Enrich.WithMachineName();
            loggerConfiguration.Enrich.WithThreadId();

            loggerConfiguration.WriteTo.Console();
#if !DEBUG
            var telemetry = services.GetRequiredService<TelemetryConfiguration>();
            var configuration = services.GetRequiredService<IConfiguration>();
            loggerConfiguration.WriteTo.ApplicationInsights(telemetry, TelemetryConverter.Traces, LogEventLevel.Information);
            loggerConfiguration.WriteTo.AzureBlobStorage(new JsonFormatter(), "StorageAccount", configuration, storageContainerName: "deliverect-logs", storageFileName: "{yyyy}/{MM}/{dd}/{HH}-log.txt", writeInBatches: true, restrictedToMinimumLevel: LogEventLevel.Debug);
#endif
        }
    }
}
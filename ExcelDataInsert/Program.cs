using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using KSCIRC.Models.Model;
using KSCIRC.Models.Settings;
using KSCIRC.Interfaces.Services;
using KSCIRC.Services;

namespace KSCIRC.ExcelDataInsert
{
    public class Program
    {
        public static IConfigurationRoot Configuration;

        static void Main(string[] args)
        {
            Console.WriteLine("Starting Excel Data Insert application...");

            var services = InitialServiceCollection();

            ConfigureServices(services);

            var provider = BuildServiceProvider(services);

            var insertData = provider.GetRequiredService<InsertData>();
            insertData.Execute();

            Console.WriteLine("Exiting...");
        }

        public static IServiceCollection InitialServiceCollection()
        {
            IServiceCollection services = new ServiceCollection();

            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Setup configuration:
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                .Build();

            ConfigureBaseServices(services, environmentName);

            services.AddLogging();

            return services;
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApplicationSettings>(Configuration.GetSection("AppSettings"));

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IConfiguration>(_ => { return Configuration; });

            services.AddTransient<IGeneService, GeneService>();
        }

        private static void ConfigureBaseServices(IServiceCollection services, string environmentName)
        {
            // Setup container here, just like a asp.net core app
            services.AddScoped(provider => new KSCIRC.Models.Mapper.MapperConfiguration().ConfigureAutoMapper(provider));

            //ditto iconfiguration
            services.AddSingleton<IConfiguration>(_ => { return Configuration; });

            //context and storage
            var connectionString = Configuration.GetConnectionString("LocalDb");
            services.AddDbContext<KSCIRC_devContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<InsertData, InsertData>();

            var cultureInfo = new System.Globalization.CultureInfo("en-US");
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }

        public static IServiceProvider BuildServiceProvider(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();

            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            return serviceProvider;
        }
    }
}

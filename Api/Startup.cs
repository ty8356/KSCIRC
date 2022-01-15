using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KSCIRC.Api.Middleware;
using KSCIRC.Interfaces.Services;
using KSCIRC.Models.Model;
using KSCIRC.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace KSCIRC.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<KSCIRC_devContext>(options => options.UseSqlServer(Configuration.GetConnectionString("LocalDb")));
            services.AddScoped(provider => new KSCIRC.Models.Mapper.MapperConfiguration().ConfigureAutoMapper(provider));

            services.AddMvc(config =>
            {
                config.Filters.Add(new HttpExceptionFilter());
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.DateFormatString = "M/d/yyyy h:mm:ss tt";
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            })
            .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);

            services.AddTransient<IGeneService, GeneService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nest;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

namespace ElasticSearchDotNet
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Log.Logger = new LoggerConfiguration()
               .Enrich.FromLogContext()
               .MinimumLevel.Verbose()
               .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(Configuration.GetValue<string>("App:ElasticSearch")))
               {
                   MinimumLogEventLevel = LogEventLevel.Verbose,
                   AutoRegisterTemplate = true
               })
               .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ElasticClient>(provider =>
            {
                var node = new Uri(Configuration.GetValue<string>("App:ElasticSearch"));
                var settings = new ConnectionSettings(node);

                return new ElasticClient(settings);
            });

            services.AddSingleton<IElasticRepository<User>, UserElasticRepository>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddSerilog();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

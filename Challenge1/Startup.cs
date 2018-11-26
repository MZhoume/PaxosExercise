using AutoMapper;
using Challenge1.Core.Filters;
using Challenge1.Data;
using Challenge1.Data.Abstractions;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using SwaggerOptions = Challenge1.Core.Swagger.SwaggerOptions;

namespace Challenge1
{
    /// <summary>
    /// Configures the service.
    /// </summary>
    public class Startup
    {
        private readonly IConfigurationSection _swaggerOptions;
        private readonly string[] _allowedEndpoints;
        private readonly string _connString;

        public Startup(IConfiguration configuration)
        {
            this._swaggerOptions = configuration.GetSection("SwaggerOptions");
            this._allowedEndpoints = configuration.GetSection("AllowedEndpoints").Get<string[]>();
            this._connString = configuration["DATABASE_CONNECTION_STRING"];
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<SwaggerOptions>(this._swaggerOptions);

            services.AddAutoMapper(options => options.CreateMissingTypeMaps = false);

            services.AddDbContextPool<ServiceContext>(options => options.UseSqlite(this._connString));
            services.AddScoped<IServiceRepository, ServiceRepository>();

            services.AddSwaggerGen(options =>
                options.SwaggerDoc("paxos-api", new Info
                {
                    Title = "Paxos Coding Challenge #1 APIs",
                    Version = "v1"
                }));

            services.AddMvcCore(options =>
                {
                    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                    options.Filters.Add(typeof(ModelStateGlobalFilter));
                })
                .AddCors()
                .AddApiExplorer()
                .AddFormatterMappings()
                .AddJsonFormatters()
                .AddJsonOptions(options =>
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver())
                .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<Startup>());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(builder =>
                builder.WithOrigins(this._allowedEndpoints)
                    .AllowAnyMethod()
                    .AllowAnyHeader());

            app.UseSwagger()
                .UseSwaggerUI(options =>
                    options.SwaggerEndpoint(this._swaggerOptions["ApiEndpoint"], "Paxos APIs"));

            app.UseMvc();
        }
    }
}

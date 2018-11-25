using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using SwaggerOptions = Challenge1.Core.Swagger.SwaggerOptions;

namespace Challenge1
{
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

            services.AddSwaggerGen(options =>
                options.SwaggerDoc("paxos-api", new Info
                {
                    Title = "Paxos Coding Challenge #1 APIs",
                    Version = "v1"
                }));
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
        }
    }
}

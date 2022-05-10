using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PokemonDex.Challenge.Clients;
using PokemonDex.Challenge.Common;
using PokemonDex.Challenge.Domain;
using PokemonDex.Challenge.Installer;
using PokemonDex.Challenge.Middleware;
using PokemonDex.Challenge.Services;

namespace PokemonDex.Challenge
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {

            Configuration = (IConfigurationRoot)configuration;
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials().Build());
            }
           );

            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddOptions();
            services.Configure<PokeApiConfig>(Configuration.GetSection("PokeApiConfig"));
            services.AddSingleton(Configuration);
            services.AddHttpClient();
            services.AddApplicationServices();
            services.AddAutoMapper(typeof(Startup));
            services.AddTransient<ExceptionHandlingMiddleWare>();
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Ignore,

            };
            services.AddSwaggerGen();
            services.AddControllers();
            services.AddApiVersioning(x =>
            {
                x.DefaultApiVersion = new ApiVersion(1, 0);
                x.AssumeDefaultVersionWhenUnspecified = true;
                x.ReportApiVersions = true;
                x.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionHandlingMiddleWare>();
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pokemon.Challenge");
            });

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PokemonDex.Challenge.Services;

namespace PokemonDex.Challenge.Test
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
           
        }

        public void ConfigureTestServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials().Build());
            }
         );
            services.AddMvc(options => options.EnableEndpointRouting = false).AddApplicationPart(typeof(Startup).Assembly);
            services.AddSingleton(Configuration);
            services.AddHttpClient();
            services.AddTransient<IPokemonService, PokemonService>();
            services.AddTransient<ITranslationService, TranslationService>();
            services.AddAutoMapper(typeof(Startup));
            // Getting Api Endpoint Urls from Config
            //var pokeapiEndpoint = Configuration.GetSection("PokeapiConfig").GetSection("PokeapiEndpoint");
            //PokeapiEndpoint = pokeapiEndpoint.Value;
            //var pokeapiTranslationEndpoint = Configuration.GetSection("PokeapiConfig").GetSection("Pokeapi_TranslationEndpoint");
            //PokeapiTranslationEndpoint = pokeapiTranslationEndpoint.Value;
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            services.AddSwaggerGen();
            services.AddControllers();

        }
    }
}

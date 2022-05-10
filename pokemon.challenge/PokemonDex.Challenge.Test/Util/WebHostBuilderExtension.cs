using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PokemonDex.Challenge.Domain;

namespace PokemonDex.Challenge.Test.Util
{
    public static class WebHostBuilderExtension
    {
        public static IWebHostBuilder CustomExtension(this IWebHostBuilder webHostBuilder)
        {
            return webHostBuilder.ConfigureServices(services =>
            {
                var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
                var connection = config.GetSection("PokeapiConfig").Get<PokeApiConfig>();
            });
        }
        public static void CustomAction(IServiceCollection services)
        {
            var config = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            config.GetSection("PokeapiConfig").Value = LoadConfiguration();
            var connection = config.GetSection("PokeapiConfig").Get<PokeApiConfig>();

        }
        private static string LoadConfiguration()
        {
            PokeApiConfig config = new PokeApiConfig()
            {
                PokeapiEndpoint = "https://pokeapi.co/api/v2/pokemon-species/",
                Pokeapi_TranslationEndpoint = "https://funtranslations.com/api/"
            };
            return JsonConvert.SerializeObject(config);
        }
    }
}

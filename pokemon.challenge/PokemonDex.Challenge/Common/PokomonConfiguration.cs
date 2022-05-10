using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PokemonDex.Challenge.Domain;

namespace PokemonDex.Challenge.Common
{
    public class PokomonConfiguration : IPokomonConfiguration
    {

        private PokeApiConfig pokeApiConfig;
        public PokomonConfiguration(IConfigurationRoot configuration)
        {

            if (configuration.GetSection("PokeapiConfig").Value == null)
            {
                pokeApiConfig = configuration.GetSection("PokeapiConfig").Get<PokeApiConfig>();

            }
            else
            {
                pokeApiConfig =
                    JsonConvert.DeserializeObject<PokeApiConfig>(configuration.GetSection("PokeapiConfig").Value);
            }
        }


        public string GetPokemonApiUrl()
        {
            return pokeApiConfig?.PokeapiEndpoint;
        }
        public string GetTranslationApiUrl()
        {
            return pokeApiConfig?.Pokeapi_TranslationEndpoint;
        }


    }
}

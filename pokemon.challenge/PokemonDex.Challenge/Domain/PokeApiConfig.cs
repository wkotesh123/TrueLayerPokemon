using Newtonsoft.Json;

namespace PokemonDex.Challenge.Domain
{
    public class PokeApiConfig
    {
        [JsonProperty]
        public string PokeapiEndpoint { get; set; }
        [JsonProperty]
        public string Pokeapi_TranslationEndpoint { get; set; }
    }
}

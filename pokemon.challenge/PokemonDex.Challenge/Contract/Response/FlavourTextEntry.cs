using Newtonsoft.Json;

namespace PokemonDex.Challenge.Contract.Response
{
    public class FlavourTextEntry
    {
        [JsonProperty("flavor_text")]
        public string FlavorText { get; set; }
        public PokemonPairSchema Language { get; set; }
        public PokemonPairSchema Version { get; set; }
    }
}

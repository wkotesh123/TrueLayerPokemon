using System.Collections.Generic;

namespace PokemonDex.Challenge.Contract.Response
{
    public class PokemonDexBasicInfoResponse
    {
        public string Name { get; set; }
        public string StandardDescription { get; set; }
        public string Habitat { get; set; }
        public bool IsLegendary { get; set; }
    }
}

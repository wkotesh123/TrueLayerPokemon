using System.Collections.Generic;

namespace PokemonDex.Challenge.Contract.Response
{
    public class PokemonApiBasicInfoResponse
    {
        public string Name { get; set; }
        public List<FlavourTextEntry> FlavourTextEntries { get; set; }
        public PokemonPairSchema HabitatDetails { get; set; }
        public bool IsLegendary { get; set; }
    }
}

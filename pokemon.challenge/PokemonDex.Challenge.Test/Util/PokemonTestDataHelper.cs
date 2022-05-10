using PokemonDex.Challenge.Contract.Response;
using System.Collections.Generic;

namespace PokemonDex.Challenge.Test.Util
{
    public class PokemonTestDataHelper
    {
        public PokemonApiBasicInfoResponse GetPokemon(string name, string habitatDetails, string description, bool isLegendary)
        {
            PokemonApiBasicInfoResponse pokemonBasicInfoResponse = new PokemonApiBasicInfoResponse()
            {
                FlavourTextEntries = new List<FlavourTextEntry>()
                {
                    new FlavourTextEntry()
                    {
                        Language = new PokemonPairSchema() {Name = "en"},
                        FlavorText = description
                    }
                },
                Name = name,
                HabitatDetails = new PokemonPairSchema() { Name = habitatDetails },
                IsLegendary = isLegendary
            };
            return pokemonBasicInfoResponse;
        }
    }
}

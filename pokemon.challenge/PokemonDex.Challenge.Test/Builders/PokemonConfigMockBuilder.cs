using Moq;
using PokemonDex.Challenge.Common;
using PokemonDex.Challenge.Domain;

namespace PokemonDex.Challenge.Test.Builders
{
    public class PokemonConfigMockBuilder
    {
        private Mock<IPokomonConfiguration> _config = new Mock<IPokomonConfiguration>();
        public Mock<IPokomonConfiguration> Build()
        {
            var pokemonConfig = Mock.Of<PokeApiConfig>(x =>
                x.PokeapiEndpoint == "https://pokeapi.co/api/v2/pokemon-species/"
                && x.Pokeapi_TranslationEndpoint == "https://api.funtranslations.com/translate/");

            _config.Setup(x => x.GetPokemonApiUrl()).Returns(pokemonConfig.PokeapiEndpoint);
            _config.Setup(x => x.GetTranslationApiUrl()).Returns(pokemonConfig.Pokeapi_TranslationEndpoint);
            return _config;
        }
       

    }
}

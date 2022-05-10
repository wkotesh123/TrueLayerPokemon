using System.Threading.Tasks;
using AutoMapper;
using Moq;
using NUnit.Framework;
using PokemonDex.Challenge.Clients;
using PokemonDex.Challenge.Common;
using PokemonDex.Challenge.Contract.Response;
using PokemonDex.Challenge.Exceptions;
using PokemonDex.Challenge.Mapper;
using PokemonDex.Challenge.Services;
using PokemonDex.Challenge.Test.Builders;
using PokemonDex.Challenge.Test.Util;

namespace PokemonDex.Challenge.Test.ServiceLayerUnitTest
{
    [TestFixture]
    public class PokemonBasicInfoServiceShould
    {
        private Mock<IPokomonConfiguration> _config = new Mock<IPokomonConfiguration>();
        private const string HabitatDetails = "cave";
        private const string Description = "It was created by\na scientist after\nyears of horrific\fgene splicing and\nDNA engineering\nexperiments.";
        private const bool IsLegendary = true;

        [SetUp]
        public void SetUp()
        {
            //Mocking app config json for servicelayer
            _config = new PokemonConfigMockBuilder().Build();


            //mapper 
            var myProfile = new BasicInfoResponseToDomainProfile();
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            mapperConfig.CreateMapper();
        }

        // get PokemonNameEmptyException 
        [TestCase("")]
        public void when_passing_empty_name_then_return_pokemon_empty_exception_response(string pokemonName)

        {
            //Arrange
            PokemonNameEmptyException pokemonNameEmptyException = new PokemonNameEmptyException();
            var mockFactory = new Mock<IPokemonApiClient>();
            mockFactory.Setup(c => c.GetPokemonBasicInfo(It.IsAny<string>())).
                                            Returns(Task.FromResult(new PokemonApiBasicInfoResponse()));

            //Act
            PokemonService pokemonApiclient = new PokemonService(mockFactory.Object);

            var pokemon = pokemonApiclient.GetBasicInfoAsync(pokemonName);
            if (pokemon.Exception != null && pokemon.Exception.InnerException is PokemonNameEmptyException)
            {
                pokemonNameEmptyException = (PokemonNameEmptyException)pokemon.Exception.InnerException;
            }
            //Assert
            Assert.AreEqual(typeof(PokemonNameEmptyException), pokemonNameEmptyException.GetType());
        }



        // "should get a pokemon basicinfo for matching name"
        [TestCase("mewtwo", ExpectedResult = "mewtwo")]
        public string when_passing_name_then_return_pokemondetails_when_found(string pokemonName)
        {
            //Arrange
            var pokemonBasicInfo =
                new PokemonTestDataHelper().GetPokemon(pokemonName, HabitatDetails, Description, IsLegendary);

            var mockFactory = new Mock<IPokemonApiClient>();
            mockFactory.Setup(c => c.GetPokemonBasicInfo(It.IsAny<string>())).
                Returns(Task.FromResult(pokemonBasicInfo));

            //Act
            PokemonService pokemonApiclient = new PokemonService(mockFactory.Object);

            var pokemon = pokemonApiclient.GetBasicInfoAsync(pokemonName);

            //Assert
            return pokemon.Result.Name;
        }



        // "should throw an Not Found exception if a pokemon Name Not found"
        [TestCase("mewtwo123")]
        public void when_passing_notMatched_name_then_return_notfound_exception_fromService(string pokemonName)
        {
            //Arrange
            NotFoundException notfoundException = new NotFoundException();

            //HttpClient Mock and Stub

            var mockHttpFactory = new PockmonHttpClientMockBuilder().ReturnNotFoundResponse();

            //Act
            IPokemonApiClient pokemanClient = new PokemonApiClient(mockHttpFactory.Object, _config.Object);

            PokemonService pokemonApiclient = new PokemonService(pokemanClient);

            var pokemon = pokemonApiclient.GetBasicInfoAsync(pokemonName);

            if (pokemon.Exception != null && pokemon.Exception.InnerException is NotFoundException)
            {
                notfoundException = (NotFoundException)pokemon.Exception.InnerException;
            }

            //Assert
            Assert.AreEqual(typeof(NotFoundException), notfoundException.GetType());
        }



        // "should throw an exception if service is unavailable"
        [TestCase("mewtwo")]
        public void when_passing_name_then_return_error_fromService_when_PokemonAPI_ServerDown(string pokemonName)
        {
            //Arrange
            ServerUnavaillableException serverUnavailableException = new ServerUnavaillableException();

            //HttpClientFactory Mock and Stub
            var mockHttpFactory = new PockmonHttpClientMockBuilder().ReturnServiceUnavailableResponse();

            //Act
            IPokemonApiClient pokemanApiClient = new PokemonApiClient(mockHttpFactory.Object, _config.Object);
            PokemonService pokemonService = new PokemonService(pokemanApiClient);
            var pokemon = pokemonService.GetBasicInfoAsync(pokemonName);
            if (pokemon.Exception != null && pokemon.Exception.InnerException is ServerUnavaillableException)
            {
                serverUnavailableException = (ServerUnavaillableException)pokemon.Exception.InnerException;
            }

            //Assert
            Assert.AreEqual(typeof(ServerUnavaillableException), serverUnavailableException.GetType());
        }

    }
}

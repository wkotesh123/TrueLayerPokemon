using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
    public class TranslationServiceWrapperShould
    {
        private Mock<IPokomonConfiguration> _config = new Mock<IPokomonConfiguration>();
        private const string PokemonName = "mewtwo";
        private const string HabitatDetails = "cave";
        private const string Description = "It was created by\na scientist after\nyears of horrific\fgene splicing and\nDNA engineering\nexperiments.";
        private const bool IsLegendary = true;

        [SetUp]
        public void SetUp()
        {
            //Mocking app config json for servicelayer
            _config = new PokemonConfigMockBuilder().Build();

            //mapper 
            var myProfile = new TranslationResponseToDomainProfile();
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            mapperConfig.CreateMapper();

            // Json Serializer for Unittest - text comparision
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        [TestCase("")]
        public void when_passing_empty_name_then_return_pokemon_empty_exception_response(string pokemonName)
        {
            //ARRANGE
            PokemonNameEmptyException pokemonNameEmptyException =null;
            var translationApiClient = new Mock<ITranslationApiClient>();
            translationApiClient.Setup(a => a.GetFunTranslationApiResponse(Description,
                    new Uri(_config.Object.GetTranslationApiUrl())))
                .ReturnsAsync(new FunTranslationApiResponse());

            var pokemonService = new Mock<IPokemonService>();
            pokemonService.Setup(a => a.GetBasicInfoAsync(pokemonName)).ReturnsAsync(new PokemonApiBasicInfoResponse());


            //ACT
            var translationService = new TranslationService(translationApiClient.Object, pokemonService.Object, _config.Object);
            var result = translationService.TranslationAsync(pokemonName);

            if (result.Exception != null && result.Exception.InnerException is PokemonNameEmptyException)
            {
                pokemonNameEmptyException = (PokemonNameEmptyException)result.Exception.InnerException;
            }

            //ASSERT
            Assert.AreEqual(typeof(PokemonNameEmptyException), pokemonNameEmptyException?.GetType());
        }

        //get a  translated text in Yoda translation
        [TestCase("mewtwo")]
        public async Task Perform_Translation_With_BasicPokemonInfo(string pokemonName)
        {
            //ARRANGE
            var pokemonBasicInfo = new PokemonTestDataHelper().GetPokemon(PokemonName, HabitatDetails,Description,IsLegendary);


            // translation api mock
            var translationApiClientMock = new Mock<ITranslationApiClient>();

            translationApiClientMock.Setup(c => c.GetFunTranslationApiResponse(It.IsAny<string>(),
                    new Uri(_config.Object.GetTranslationApiUrl()))).
                ReturnsAsync(new FunTranslationApiResponse());


            var pokemonServiceMock = new Mock<IPokemonService>();
            pokemonServiceMock.Setup(a => a.GetBasicInfoAsync(pokemonName)).ReturnsAsync(pokemonBasicInfo);
            

            //Action
            TranslationService transService = new TranslationService(translationApiClientMock.Object, pokemonServiceMock.Object, _config.Object);

            var translatedResult = await transService.TranslationAsync(pokemonName);

            //Assert
            Assert.AreEqual(Description, translatedResult.TranslatedDescription);
        }

        //get a  translated text in Shakespeare translation
        [TestCase("mewtwo")]
        public async Task Perform_shakespeare_Translation_With_islegendary_false(string pokemonName)
        {
            //Arrange
            var pokemonBasicInfo = new PokemonTestDataHelper().GetPokemon(pokemonName, "rare", Description, false);

            // translation api mock
            var translationApiClientMock = new Mock<ITranslationApiClient>();

            translationApiClientMock.Setup(c => c.GetFunTranslationApiResponse(It.IsAny<string>(),
                    new Uri(_config.Object.GetTranslationApiUrl()))).
                ReturnsAsync(new FunTranslationApiResponse());

            var pokemonServiceMock = new Mock<IPokemonService>();
            pokemonServiceMock.Setup(a => a.GetBasicInfoAsync(pokemonName)).ReturnsAsync(pokemonBasicInfo);


            //Action
            TranslationService transService = new TranslationService(translationApiClientMock.Object, pokemonServiceMock.Object, _config.Object);

            var translatedResult = await transService.TranslationAsync(pokemonName);

            //Assert
            Assert.AreEqual(Description, translatedResult.TranslatedDescription);
        }

        //get a too many request error while too many request sent
        [TestCase()]
        public void When_translation_fails_then_tranlation_description_matches_base_description()
        {

            //ARRANGE
            TooManyRequestException tooManyRequestException = null;
            var pokemonBasicInfo = new PokemonTestDataHelper().GetPokemon(PokemonName, HabitatDetails, Description, IsLegendary);

            var httpClientFactoryMock = new TranslationsHttpClientMockBuilder().Return_ToomanyRequest_Response();
            
            var translationApiClient = new TranslationApiClient(httpClientFactoryMock.Object);

            var pokemonServiceMock = new Mock<IPokemonService>();
            pokemonServiceMock.Setup(a => a.GetBasicInfoAsync(PokemonName)).ReturnsAsync(pokemonBasicInfo);

            //ACTION
            TranslationService transService = new TranslationService(translationApiClient, pokemonServiceMock.Object, _config.Object);

            var translatedResult = transService.TranslationAsync(PokemonName);

         

            //ASSERT
            Assert.AreEqual(Description, translatedResult.Result.TranslatedDescription);
        }

        //get a service unavailable
        [TestCase("mewtwo")]
        public void when_passing_name_then_return_OKResponse_fromService_when_PokemonAPI_ServerDown(string pokemonName)
        {

            //ARRANGE
            
            var pokemonBasicInfo = new PokemonTestDataHelper().GetPokemon(PokemonName, HabitatDetails, Description, IsLegendary);

            var httpClientFactoryMock = new TranslationsHttpClientMockBuilder().Return_OkResponse();

            var translationApiClient = new TranslationApiClient(httpClientFactoryMock.Object);

            var pokemonServiceMock = new Mock<IPokemonService>();
            pokemonServiceMock.Setup(a => a.GetBasicInfoAsync(PokemonName)).ReturnsAsync(pokemonBasicInfo);

            //ACTION
            TranslationService transService = new TranslationService(translationApiClient, pokemonServiceMock.Object, _config.Object);

            var translatedResult = transService.TranslationAsync(PokemonName);
            int responseCode = 0;
            if (translatedResult.Exception == null)
            {
                responseCode = (int)HttpStatusCode.OK;
            }

            //ASSERT
            Assert.AreEqual((int)HttpStatusCode.OK, responseCode);
        }


    }
}

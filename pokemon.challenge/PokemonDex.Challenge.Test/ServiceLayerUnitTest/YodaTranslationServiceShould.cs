using AutoMapper;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using PokemonDex.Challenge.Clients;
using PokemonDex.Challenge.Common;
using PokemonDex.Challenge.Contract.Response;
using PokemonDex.Challenge.Mapper;
using PokemonDex.Challenge.Services;
using PokemonDex.Challenge.Test.Builders;
using System;
using System.Threading.Tasks;

namespace PokemonDex.Challenge.Test.ServiceLayerUnitTest
{
    [TestFixture]
    public class YodaTranslationServiceShould
    {
        private Mock<IPokomonConfiguration> _config = new Mock<IPokomonConfiguration>();
        
        private const string Description = "It was created ";
        private const string YodaTranslatedText = "This is Test Text";
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

        [TestCase("mewtwo")]
        public async Task Perform_Yoda_Translation_With_BasicPokemonInfo(string pokemonName)
        {
            //Arrange
            var translationApiClientMock = new Mock<ITranslationApiClient>();
            var funTranslationApiResponse = new FunTranslationApiResponse()
            {
                Success = new Success() { Total = 1 },
                Contents = new Contents()
                    { Translated = YodaTranslatedText, Text = "Yoda", Translation = "Yoda" }
            };
         
            translationApiClientMock.Setup(c => c.GetFunTranslationApiResponse(It.IsAny<string>(), 
                It.IsAny<Uri>())).ReturnsAsync(funTranslationApiResponse);

            //Action
            var transService = new YodaTranslationService(translationApiClientMock.Object,_config.Object);

            var translatedResult = await transService.TranslateAsync(Description);
         
            //Assert
            Assert.AreEqual(YodaTranslatedText, translatedResult.Contents.Translated);
        }

    





    }
}

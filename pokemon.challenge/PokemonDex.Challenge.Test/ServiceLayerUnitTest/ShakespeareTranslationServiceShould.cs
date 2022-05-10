using System.Threading.Tasks;
using AutoMapper;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
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
    public class ShakespeareTranslationServiceShould
    {
        private Mock<IPokomonConfiguration> _config = new Mock<IPokomonConfiguration>();

        [SetUp]
        public void SetUp()
        {
            //GetConfigMock 
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

        //get a text translated in Shakespeare language
        [TestCase("'t wast did create by a scientist after years of horrific gene splicing and dna engineering experiments.")]
        public async Task When_passing_Description_then_get_translated_text(string description)
        {
            //Arrange
            var mockFactory = new TranslationsClientMockBuilder().Builder(description);

            //Action
            ShakespeareTranslationService service = new ShakespeareTranslationService(mockFactory.Object, _config.Object);
            var translatedResult = await service.TranslateAsync(description);
            
            //Expected results
            var loadTxtFromFile = new LoadJsonFor().ShakespeareTranslationResponseStub();
            var expectedFileResult = JsonConvert.DeserializeObject<FunTranslationApiResponse>(loadTxtFromFile);

            //Assert
            Assert.AreEqual(expectedFileResult?.Contents.Translated.Trim(), translatedResult.Contents.Translated.Trim());
        }


        //get a too many request error while too many request sent
        [TestCase("It was created by\na scientist after\nyears of horrific\fgene splicing and\nDNA engineering\nexperiments.")]
        public void When_Toomany_request_Goes_then_handle_toomanyrequest_exception(string description)
        {
            //Arrange
            TooManyRequestException tooManyRequestException = new TooManyRequestException();
            var mockFactory = new TranslationsClientMockBuilder().Builder(description);

            //Action
            ShakespeareTranslationService service = new ShakespeareTranslationService(mockFactory.Object, _config.Object);
            var translatedResult = service.TranslateAsync(description);
            if (translatedResult.Exception?.InnerException is TooManyRequestException)
            {
                tooManyRequestException = (TooManyRequestException)translatedResult.Exception.InnerException;
            }



            //Assert
            Assert.AreEqual(typeof(TooManyRequestException), tooManyRequestException.GetType());


        }


    }
}

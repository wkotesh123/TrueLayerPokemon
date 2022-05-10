using AutoMapper;
using Moq;
using NUnit.Framework;
using PokemonDex.Challenge.Contract.Response;
using PokemonDex.Challenge.Controllers;
using PokemonDex.Challenge.Mapper;
using PokemonDex.Challenge.Services;
using System.Threading.Tasks;

namespace PokemonDex.Challenge.Test.ControllerTest
{
    [TestFixture]
    public class TranslationControllerShould
    {
   

        [SetUp]
        public void SetUp()
        {
            //mapper 
            var myProfile = new TranslationResponseToDomainProfile();
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            mapperConfig.CreateMapper();
        }

        [TestCase("mewtwo")]
        public void TranslationController_return_Translated_Response(string name)
        {
            // Arrange
            var mockRepo = new Mock<ITranslationService>();
            mockRepo.Setup(repo => repo.TranslationAsync(name))
                .Returns(Task.FromResult(new PokemonDexTranslationResponse()
                {
                    Name = name,
                    Habitat = "cave",
                    IsLegendary = true,
                    TranslatedDescription = "'t wast did create by a scientist after years of horrific gene splicing and dna engineering experiments."
                }));

            //Action
            var controller = new TranslationController(mockRepo.Object);
            var controllerResponse = controller.GetTranslation(name);
            var result = controllerResponse.Result;
            var apiResponse = result?.Value as PokemonDexTranslationResponse;
            var expectedControllerresponse = new PokemonDexTranslationResponse()
            {
                TranslatedDescription = "'t wast did create by a scientist after years of horrific gene splicing and dna engineering experiments.",
            };

            //Assert
            Assert.AreEqual(expectedControllerresponse.TranslatedDescription, apiResponse?.TranslatedDescription);

        }
    }
}

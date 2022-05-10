using AutoMapper;
using Moq;
using NUnit.Framework;
using PokemonDex.Challenge.Contract.Response;
using PokemonDex.Challenge.Controllers;
using PokemonDex.Challenge.Mapper;
using PokemonDex.Challenge.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PokemonDex.Challenge.Test.Util;

namespace PokemonDex.Challenge.Test.ControllerTest
{
    [TestFixture]
    public class PokemonDexControllerShould
    {
        private static IMapper _mapper;
        private const string HabitatDetails = "cave";
        private const string Description = "It was created by\na scientist after\nyears of horrific\fgene splicing and\nDNA engineering\nexperiments.";
        private const bool IsLegendary = true;
        [SetUp]
        public void SetUp()
        {
            //mapper 
            var myProfile = new BasicInfoResponseToDomainProfile();
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = mapperConfig.CreateMapper();
        }

        [TestCase("mewtwo")]
        public async Task PokemonDexController_return_BasicInfo_Response(string name)
        {
            // Arrange
            var mockRepo = new Mock<IPokemonService>();
            var pokemonBasicInfo =
                new PokemonTestDataHelper().GetPokemon(name, HabitatDetails, Description, IsLegendary);

            mockRepo.Setup(repo => repo.GetBasicInfoAsync(name))
                .ReturnsAsync(pokemonBasicInfo);
            //Action
            var controller = new PokemonDexController(mockRepo.Object, _mapper);
            var controllerResponse = await controller.GetBasicPokemonInfo(name);

            var result = controllerResponse.Result as OkObjectResult;

            var apiResponse = result?.Value as PokemonDexBasicInfoResponse;
            
            var expectedControllerresponse = new PokemonDexBasicInfoResponse()
            {
                StandardDescription = Description
            };

            //Assert
            Assert.AreEqual(expectedControllerresponse.StandardDescription.First(), apiResponse?.StandardDescription.First());

        }
    }
}

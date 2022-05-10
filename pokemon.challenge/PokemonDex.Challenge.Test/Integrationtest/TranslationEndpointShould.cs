using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NUnit.Framework;
using PokemonDex.Challenge.Exceptions;
using PokemonDex.Challenge.Test.Util;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PokemonDex.Challenge.Test.IntegrationTest
{
    [TestFixture]
    public class TranslationEndpointShould
    {
        [SetUp]
        public void Setup()
        {

            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            WebHostBuilder = new WebHostBuilder()
                   .UseConfiguration(config)
                     .CustomExtension()
                   .ConfigureServices(WebHostBuilderExtension.CustomAction)
           .UseEnvironment("Test")
           .UseStartup<TestStartup>();

        }
        public IWebHostBuilder WebHostBuilder { get; set; }

        //   Not Found response from Controller
        [TestCase("mewtwo", "​api/PokemonDex?pokemonName=mewtwo")]
        public void Return_NotFound_Response_For_Given_PokemonName(string pokemonName, string url)
        {
            var notfoundException = new NotFoundException();
            var actualResponse = HitBasicInfoEndpointAndReturn_404Response();
            if (actualResponse.Exception is {InnerException: NotFoundException exception})
            {
                notfoundException = exception;
            }
            Assert.AreEqual(typeof(NotFoundException), notfoundException.GetType());
        }


        private async Task<HttpResponseMessage> HitBasicInfoEndpointAndReturn_404Response()
        {

            using (var server = new TestServer(WebHostBuilder))
            using (var client = server.CreateClient())
            {
                var request = "mewtwo123";
                var json = JsonConvert.SerializeObject(request);
                var data = new StringContent(json, Encoding.Default, "application/json");
                var controllerResult = await client.PostAsync("api/Translation?pokemonName=mewtwo123", data);
                controllerResult.EnsureSuccessStatusCode();
                return controllerResult;
            }
        }
    }

}

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using PokemonDex.Challenge.Test.Util;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PokemonDex.Challenge.Test.IntegrationTest
{
    [TestFixture]
    public class GetBasicInfoEndpointShould
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
                   .ConfigureServices(services => WebHostBuilderExtension.CustomAction(services))
           .UseEnvironment("Test")
           .UseStartup<Startup>();

        }
        public IWebHostBuilder WebHostBuilder { get; set; }

        //Ok response from Controller
        [TestCase("mewtwo", "http://localhost:5000/api/pokemon/mewtwo")]
        public async Task Return_Ok_Response_For_Given_PokemonName(string pokemonName, string url)
        {
            var actualResponse = await HitBasicInfoEndpointAndReturn_OKResponse(url);
            Assert.AreEqual(HttpStatusCode.OK, actualResponse.StatusCode);
        }

        //   Not Found response from Controller
        [TestCase("mewtwo", "http://localhost:5000/api/pokemon/mewtwo1")]
        public async Task Return_NotFound_Response_For_Given_PokemonName(string pokemonName, string url)
        {
            var actualResponse = await HitBasicInfoEndpointAndReturn_404Response(url);
            Assert.AreEqual(HttpStatusCode.NotFound, actualResponse.StatusCode);
        }

        private async Task<HttpResponseMessage> HitBasicInfoEndpointAndReturn_OKResponse(string url)
        {

            using (var server = new TestServer(WebHostBuilder))
            {
                using (var client = server.CreateClient())
                {
                    var controllerResult = await client.GetAsync(new Uri(url));
                    controllerResult.EnsureSuccessStatusCode();
                    return controllerResult;
                }
            }
        }
        private async Task<HttpResponseMessage> HitBasicInfoEndpointAndReturn_404Response(string url)
        {
            using (var server = new TestServer(WebHostBuilder))
            {
                using (var client = server.CreateClient())
                {
                    var controllerResult = await client.GetAsync(new Uri(url));
                    return controllerResult;
                }
            }
        }

       
    }

}

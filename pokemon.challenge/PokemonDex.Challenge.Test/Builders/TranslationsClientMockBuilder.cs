using Moq;
using PokemonDex.Challenge.Clients;
using PokemonDex.Challenge.Contract.Response;
using System;
using System.Threading.Tasks;

namespace PokemonDex.Challenge.Test.Builders
{
    public class TranslationsClientMockBuilder
    {
        public Mock<ITranslationApiClient> Builder(string description)
        {
            var funTranslationApiResponse = new FunTranslationApiResponse()
            {
                Success = new Success() { Total = 1 },
                Contents = new Contents()
                    { Translated = description, Text = "Yoda", Translation = "Yoda" }
            };
            var mockTranslationClient = new Mock<ITranslationApiClient>();

            mockTranslationClient.Setup(c => c.GetFunTranslationApiResponse(It.IsAny<string>(),
                    It.IsAny<Uri>())).
                Returns(Task.FromResult(funTranslationApiResponse));

            return mockTranslationClient;

        }
    }
}

using Moq;
using Moq.Protected;
using PokemonDex.Challenge.Test.Util;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PokemonDex.Challenge.Test.Builders
{
    public class TranslationsHttpClientMockBuilder
    {
        public Mock<IHttpClientFactory> Return_OkResponse()
        {
            var mockHttpFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(new LoadJsonFor().ShakespeareTranslationResponseStub()),
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            mockHttpFactory.Setup(c => c.CreateClient(It.IsAny<string>())).Returns(client);
            return mockHttpFactory;

        }
        public Mock<IHttpClientFactory> Return_ToomanyRequest_Response()
        {
            var mockHttpFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.TooManyRequests,
                    Content = new StringContent(string.Empty),
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            mockHttpFactory.Setup(c => c.CreateClient(It.IsAny<string>())).Returns(client);
            return mockHttpFactory;

        }
    
    }
}

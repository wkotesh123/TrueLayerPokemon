using Moq;
using Moq.Protected;
using PokemonDex.Challenge.Test.Util;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PokemonDex.Challenge.Test.Builders
{
    public class PockmonHttpClientMockBuilder
    {
       public Mock<IHttpClientFactory> ReturnNotFoundResponse()
        {
            var mockHttpFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent(string.Empty),
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            mockHttpFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
            return mockHttpFactory;
        }
        public Mock<IHttpClientFactory> ReturnServiceUnavailableResponse()
        {
            var mockHttpFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.ServiceUnavailable,
                    Content = new StringContent(string.Empty),
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            mockHttpFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
            return mockHttpFactory;
        }
    }
}

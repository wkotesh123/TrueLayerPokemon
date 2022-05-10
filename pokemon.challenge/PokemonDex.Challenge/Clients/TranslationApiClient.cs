using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PokemonDex.Challenge.Contract.Request;
using PokemonDex.Challenge.Contract.Response;
using PokemonDex.Challenge.Exceptions;

namespace PokemonDex.Challenge.Clients
{
    public class TranslationApiClient : ITranslationApiClient
    {
        readonly IHttpClientFactory _httpClientFactory;
        public TranslationApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

        }
        public async Task<FunTranslationApiResponse> GetFunTranslationApiResponse(string description, Uri endpointAddress)
        {

            using (var translationApiClient = _httpClientFactory.CreateClient())
            {

                TranslationRequest request = new TranslationRequest()
                {
                    Text = description
                };
                
                var json = JsonConvert.SerializeObject(request);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                translationApiClient.DefaultRequestHeaders.Accept.Clear();
                translationApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await translationApiClient.PostAsync(endpointAddress, data);
                var translationApiResponse = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var funTranslationApiResponse = JsonConvert.DeserializeObject<FunTranslationApiResponse>(translationApiResponse);
                    return funTranslationApiResponse;
                }
                else
                {
                    return new FunTranslationApiResponse()
                    {
                        Contents = new Contents()
                        {
                            Text = description,
                            Translated = description,
                            Translation = description
                        },
                        Success = new Success()
                        {
                            Total = 1
                        }
                    };

                }
            }



        }
    }
}

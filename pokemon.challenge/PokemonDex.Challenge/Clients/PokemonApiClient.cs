using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PokemonDex.Challenge.Common;
using PokemonDex.Challenge.Contract.Response;
using PokemonDex.Challenge.Exceptions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PokemonDex.Challenge.Clients
{
    public class PokemonApiClient : IPokemonApiClient
    {
        readonly IHttpClientFactory _httpClientFactory;
        private IPokomonConfiguration _config;
        public PokemonApiClient(IHttpClientFactory httpClientFactory, IPokomonConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }
        public async Task<PokemonApiBasicInfoResponse> GetPokemonBasicInfo(string pokemonName)
        {
            using (var pokemonApiClient = _httpClientFactory.CreateClient())
            {
                pokemonApiClient.BaseAddress = new Uri(_config.GetPokemonApiUrl());
                using (HttpResponseMessage response = await pokemonApiClient.GetAsync(pokemonName))
                {
                    using (HttpContent content = response.Content)
                    {
                        string result = await content.ReadAsStringAsync();
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            JObject rss = JObject.Parse(result);
                            var pokemonApiBasicInfoResponse = new PokemonApiBasicInfoResponse()
                            {
                                Name = (string)rss["name"],
                                HabitatDetails = JsonConvert.DeserializeObject<PokemonPairSchema>(rss["habitat"]?.ToString() ?? string.Empty),
                                IsLegendary = (bool)rss["is_legendary"],
                                FlavourTextEntries = JsonConvert.DeserializeObject<List<FlavourTextEntry>>(rss["flavor_text_entries"]?.ToString() ?? string.Empty)
                            };

                            return pokemonApiBasicInfoResponse;
                        }

                        else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            throw new NotFoundException();

                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
                        {
                            throw new ServerUnavaillableException();
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                }



            }
        }
    }
}

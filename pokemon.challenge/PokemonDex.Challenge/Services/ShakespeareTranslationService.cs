using Microsoft.Extensions.Configuration;
using PokemonDex.Challenge.Common;
using PokemonDex.Challenge.Contract.Response;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using PokemonDex.Challenge.Clients;

namespace PokemonDex.Challenge.Services
{

    public class ShakespeareTranslationService
    {
        readonly ITranslationApiClient _translationClient;
        readonly IPokomonConfiguration _config;

        private string _baseUri;

        public ShakespeareTranslationService(ITranslationApiClient translationClient, IPokomonConfiguration configuration)
        {
            _translationClient = translationClient;
            _config = configuration;
        }

        public async Task<FunTranslationApiResponse> TranslateAsync(string description)
        {
            _baseUri = _config.GetTranslationApiUrl() + "shakespeare";
            return await _translationClient.GetFunTranslationApiResponse(description, new Uri(_baseUri));
        }


    }
}
using log4net;
using PokemonDex.Challenge.Clients;
using PokemonDex.Challenge.Common;
using PokemonDex.Challenge.Contract.Response;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace PokemonDex.Challenge.Services
{

    public class YodaTranslationService
    {
        private ITranslationApiClient _translationClient;
        private IPokomonConfiguration _config;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()?.DeclaringType);
        private string _baseUri;

        public YodaTranslationService(ITranslationApiClient translationClient, IPokomonConfiguration configuration)
        {

            _translationClient = translationClient;
            _config = configuration;

        }

        public async Task<FunTranslationApiResponse> TranslateAsync(string description)
        {
             _baseUri = _config.GetTranslationApiUrl() + "yoda";
             return await _translationClient.GetFunTranslationApiResponse(description, new Uri(_baseUri));
        }


    }

}
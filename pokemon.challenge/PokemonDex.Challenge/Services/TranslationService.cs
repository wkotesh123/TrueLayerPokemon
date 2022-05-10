using log4net;
using Microsoft.Extensions.Configuration;
using PokemonDex.Challenge.Contract.Response;
using PokemonDex.Challenge.Exceptions;
using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using PokemonDex.Challenge.Clients;
using PokemonDex.Challenge.Common;

namespace PokemonDex.Challenge.Services
{

    public class TranslationService : ITranslationService
    {
        private ITranslationApiClient _translationClient;
        private static readonly ILog Log = LogManager.GetLogger("TranslationService");
        private IPokemonService _pokemonService;
        readonly IPokomonConfiguration _configuration;

        public TranslationService(ITranslationApiClient translationClient, IPokemonService pokemonService, IPokomonConfiguration configuration)
        {
            _translationClient = translationClient;
            _pokemonService = pokemonService;
            _configuration = configuration;
        }
        public async Task<PokemonDexTranslationResponse> TranslationAsync(string pokemonName)
        {
                Log.Info("Translation started");
            
                if (string.IsNullOrEmpty(pokemonName))
                {
                    throw new PokemonNameEmptyException();
                }
                PokemonDexTranslationResponse response = new PokemonDexTranslationResponse();
                var basicInfoResponse = await _pokemonService.GetBasicInfoAsync(pokemonName);
                response.Name = pokemonName;
                response.Habitat = basicInfoResponse.HabitatDetails?.Name;
                response.IsLegendary = basicInfoResponse.IsLegendary;
                if (basicInfoResponse.FlavourTextEntries != null && basicInfoResponse.FlavourTextEntries.Any())
                {
                    if (isYodaTranslationApplicable(basicInfoResponse))
                    {
                        YodaTranslationService yodaService = new YodaTranslationService(_translationClient, _configuration);
                        var translationResponse = await yodaService.TranslateAsync(basicInfoResponse.FlavourTextEntries.First().FlavorText);
                        response.TranslatedDescription = string.IsNullOrEmpty(translationResponse?.Contents.Translated) ?
                                                                    basicInfoResponse.FlavourTextEntries?.First().FlavorText :
                                                                    translationResponse.Contents.Translated;
                        Log.Info("YodaTranslationService Response Received");
                        return response;
                    }
                    else
                    {
                        ShakespeareTranslationService shakespeare = new ShakespeareTranslationService(_translationClient, _configuration);
                        // took first standnard Text for simplicity purpose
                        var translationResponse = await shakespeare.TranslateAsync(basicInfoResponse.FlavourTextEntries.First().FlavorText);
                        response.TranslatedDescription = string.IsNullOrEmpty(translationResponse?.Contents.Translated) ?
                                                                     basicInfoResponse.FlavourTextEntries?.First().FlavorText :
                                                                     translationResponse.Contents.Translated;
                        Log.Info("ShakespeareTranslationService Response Received");
                        return response;
                    
                    }
                }
                else
                {
                    response.TranslatedDescription = basicInfoResponse.Name;
                    return response; // for not able to translate for some reason
                }
            
           

        }
        private bool isYodaTranslationApplicable(PokemonApiBasicInfoResponse basicInfoResponse)
        {
            if (basicInfoResponse.HabitatDetails?.Name == "cave" || basicInfoResponse.IsLegendary == true)
            {
                return true;
            }
            return false;
        }

    }
}
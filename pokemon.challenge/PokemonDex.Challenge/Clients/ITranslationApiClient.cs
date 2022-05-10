using System;
using System.Threading.Tasks;
using PokemonDex.Challenge.Contract.Response;

namespace PokemonDex.Challenge.Clients
{
    public interface ITranslationApiClient
    {
        Task<FunTranslationApiResponse> GetFunTranslationApiResponse(string description, Uri endpointAddress);
    }
}

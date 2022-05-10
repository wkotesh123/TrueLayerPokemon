using System.Threading.Tasks;
using PokemonDex.Challenge.Contract.Response;

namespace PokemonDex.Challenge.Services
{
    public interface ITranslationService
    {
        Task<PokemonDexTranslationResponse> TranslationAsync(string pokemonName);

    }
}

using PokemonDex.Challenge.Contract.Response;
using System.Threading.Tasks;

namespace PokemonDex.Challenge.Services
{
    public interface IPokemonService
    {
        Task<PokemonApiBasicInfoResponse> GetBasicInfoAsync(string pokemonName);
    }
}

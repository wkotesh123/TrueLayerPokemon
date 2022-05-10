using PokemonDex.Challenge.Contract.Response;
using System.Threading.Tasks;

namespace PokemonDex.Challenge.Clients
{
    public interface IPokemonApiClient
    {
        public Task<PokemonApiBasicInfoResponse> GetPokemonBasicInfo(string name);

    }
}

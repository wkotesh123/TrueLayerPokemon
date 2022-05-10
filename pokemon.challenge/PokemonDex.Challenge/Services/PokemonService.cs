using PokemonDex.Challenge.Clients;
using PokemonDex.Challenge.Contract.Response;
using PokemonDex.Challenge.Exceptions;
using System.Threading.Tasks;

namespace PokemonDex.Challenge.Services
{
    public class PokemonService : IPokemonService
    {
        private IPokemonApiClient _pokemonClient;

        public PokemonService(IPokemonApiClient pokemonClient)
        {
            _pokemonClient = pokemonClient;
        }

        public async Task<PokemonApiBasicInfoResponse> GetBasicInfoAsync(string pokemonName)
        {

            if (string.IsNullOrEmpty(pokemonName))
            {
                throw new PokemonNameEmptyException();
            }
            return await _pokemonClient.GetPokemonBasicInfo(pokemonName);

        }


    }
}

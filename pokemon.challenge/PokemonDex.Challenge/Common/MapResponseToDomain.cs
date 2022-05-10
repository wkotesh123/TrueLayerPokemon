using AutoMapper;
using PokemonDex.Challenge.Contract.Response;

namespace PokemonDex.Challenge.Common
{
    public class MapResponseToDomain
    {
        readonly IMapper _mapper;
        public MapResponseToDomain(IMapper mapper)
        {
            _mapper = mapper;
        }
        public PokemonDexBasicInfoResponse MapBasicInfoResponseToDomain(PokemonApiBasicInfoResponse pokemonApiBasicInfoResponse)
        {
            var response = _mapper.Map<PokemonDexBasicInfoResponse>(pokemonApiBasicInfoResponse);
            return response;
        }
    }
}

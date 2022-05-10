using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonDex.Challenge.Common;
using PokemonDex.Challenge.Contract.Response;
using PokemonDex.Challenge.Services;
using System.Threading.Tasks;

namespace PokemonDex.Challenge.Controllers
{
    [ApiController]
    [ApiVersion("1")]

    [Route("pokemon")]
    [Route("api/pokemon")]
    [Route("api/v{v:apiVersion}/pokemon")]
    [Route("api/[controller]")]
    public class PokemonDexController : ControllerBase
    {
        readonly IPokemonService _pokemonService;
        private readonly IMapper _mapper;
        public PokemonDexController(IPokemonService pokemonService, IMapper mapper)
        {
            _pokemonService = pokemonService;
            _mapper = mapper;

        }
        [HttpGet]
        [Route("{pokemonName}")]
        public virtual async Task<ActionResult<PokemonDexBasicInfoResponse>> GetBasicPokemonInfo(string pokemonName)
        {

            var pokemonApiBasicInfoResponse = await _pokemonService.GetBasicInfoAsync(pokemonName);

            MapResponseToDomain domainMapper = new MapResponseToDomain(_mapper);

            var pokemonDexBasicInfoResponse = domainMapper.MapBasicInfoResponseToDomain(pokemonApiBasicInfoResponse);

            return Ok(pokemonDexBasicInfoResponse);
        }

    }

}


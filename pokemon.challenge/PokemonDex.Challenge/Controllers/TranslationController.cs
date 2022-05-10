using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonDex.Challenge.Contract.Response;
using PokemonDex.Challenge.Exceptions;
using PokemonDex.Challenge.Services;
using System;
using System.Threading.Tasks;

namespace PokemonDex.Challenge.Controllers
{
  
    [ApiController]
    [ApiVersion("1")]
    [Route("/pokemon/Translated")]
    [Route("/api/v{v:apiVersion}/pokemon/Translated")]
    [Route("/api/pokemon/Translated")]
    [Route("api/[controller]")]
    public class TranslationController : ControllerBase
    {
        readonly ITranslationService _service;

        public TranslationController(ITranslationService translationService)
        {
            _service = translationService;
        }
        [HttpGet]
        [Route("{pokemonName}")]
        public virtual async Task<ActionResult<PokemonDexTranslationResponse>> GetTranslation(string pokemonName)
        {
            
                var funTranslationApiResponse = await _service.TranslationAsync(pokemonName);

                return funTranslationApiResponse;
            
        }
    }
}

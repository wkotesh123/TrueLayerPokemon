using AutoMapper;
using PokemonDex.Challenge.Contract.Response;

namespace PokemonDex.Challenge.Mapper
{
    public class TranslationResponseToDomainProfile : Profile
    {
        public TranslationResponseToDomainProfile()
        {
            CreateMap<FunTranslationApiResponse, PokemonDexTranslationResponse>()
               //.ForMember(dest => dest.Name, src => src.MapFrom(s => s.Name))
               //.ForMember(dest => dest.IsLegendary, src => src.MapFrom(s => s.IsLegendary))
               .ForMember(dest => dest.TranslatedDescription, src => src.MapFrom(s => s.Contents.Translated));
            // .ForMember(dest => dest.Habitat, src => src.MapFrom(s => s.HabitatDetails));

        }
    }
}

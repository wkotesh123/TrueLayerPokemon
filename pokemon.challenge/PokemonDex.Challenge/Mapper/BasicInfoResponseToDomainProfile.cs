using System;
using AutoMapper;
using PokemonDex.Challenge.Contract.Response;
using System.Linq;
using Newtonsoft.Json;

namespace PokemonDex.Challenge.Mapper
{
    public class BasicInfoResponseToDomainProfile : Profile
    {
        public BasicInfoResponseToDomainProfile()
        {
            
            CreateMap<PokemonApiBasicInfoResponse, PokemonDexBasicInfoResponse>()
               .ForMember(dest => dest.Name, src => src.MapFrom(s => s.Name))
               .ForMember(dest => dest.IsLegendary, src => src.MapFrom(s => s.IsLegendary))
               .ForMember(dest => dest.StandardDescription, src => src.MapFrom(s => s.FlavourTextEntries.Where(a=>a.Language.Name=="en").Take(1).Select(a => a.FlavorText).FirstOrDefault().Replace("\r", " ").Replace("\n", " ")))
               .ForMember(dest => dest.Habitat, src => src.MapFrom(s => s.HabitatDetails.Name));

        }
    }
}

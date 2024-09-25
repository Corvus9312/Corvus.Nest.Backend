using AutoMapper;
using Corvus.Nest.Backend.Models.DAL.Corvus;
using Corvus.Nest.Backend.ViewModels;

namespace Corvus.Nest.Backend.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<About, GetAboutVM>();
    }
}
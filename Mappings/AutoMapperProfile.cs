using AutoMapper;
using NzWalks.Models.Domain;
using NzWalks.Models.Dto;

namespace NzWalkspace.Mapping{
public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles(){
            CreateMap<Region, RegionDto>().ReverseMap();
            CreateMap<AddRegionRequestDto, Region>().ReverseMap();

            CreateMap<AddWalksRequestDto,Walk>().ReverseMap();
            CreateMap<Walk, WalkDto>().ReverseMap();
        }
    
}}
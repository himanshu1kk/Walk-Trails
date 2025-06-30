
using System;
using System.Collections.Generic;
using NzWalks.Models.Domain;
using NzWalks.Models.DTO;
namespace NzWalks.Service.Attract;

public interface IAttractionService
{

    Task<Attraction> CreateAttractionAsync(AttractionInteractionDto attractionDto, string userId);
    Task<List<AttractionDto>> GetAllAttractionsAsync();
    Task<AttractionDto> GetAttractionByIdAsync(string id);
    Task<List<AttractionDto>> SearchAttractionsAsync(string? searchTerm, string? searchBy);
    

//     Task<AttractionsModel> GetWalkByIdAsync(string Id);

    //     Task CreateWalkAsync(AttractionsModel attraction);

    //     Task<List<AttractionsModel>> GetAllWalks();



}
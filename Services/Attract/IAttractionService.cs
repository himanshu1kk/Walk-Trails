
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

    Task<Attraction> UpdateAttractionAsync(UpdateAttractionDto attractionDto, string userId);
    Task<Attraction> UpvoteAttractionAsync(string attractionId, string userId);
    Task<Attraction> AddOrUpdateStudentTipAsync(string attractionId, string studentTip);
    Task<Attraction> AddOrUpdateFavoritesAsync(string attractionId, int favorites);

    Task<Location> UpdateAttractionLocationAsync(UpdateLocationDto locationDto);

    Task<List<AttractionDto>> GetAttractionsByUserIdAsync(string userId);

//     Task<AttractionsModel> GetWalkByIdAsync(string Id);

    //     Task CreateWalkAsync(AttractionsModel attraction);

    //     Task<List<AttractionsModel>> GetAllWalks();



}
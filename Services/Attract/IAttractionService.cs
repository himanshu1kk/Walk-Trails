
using System;
using System.Collections.Generic;
using NzWalks.Models.Domain;
using NzWalks.Models.DTO;
namespace NzWalks.Service.Attract;

public interface IAttractionService
{

    Task<Attraction> CreateAttractionAsync(AttractionInteractionDto attractionDto, string userId);
    Task<PaginatedDto> GetAllAttractionsAsync(int pageNumber , int pageSize);
    Task<AttractionDto> GetAttractionByIdAsync(string id);
    Task<PaginatedDto> SearchAttractionsAsync(string? searchTerm, string? searchBy , int pageNumber , int pageSize);

    Task<Attraction> UpdateAttractionAsync(UpdateAttractionDto attractionDto, string userId);
    Task<Attraction> UpvoteAttractionAsync(string attractionId, string userId);
    Task<Attraction> AddOrUpdateStudentTipAsync(string attractionId, string studentTip);
    Task<Attraction> AddOrUpdateFavoritesAsync(string attractionId, int favorites);

    Task<Location> UpdateAttractionLocationAsync(UpdateLocationDto locationDto);

    Task<List<AttractionDto>> GetAttractionsByUserIdAsync(string userId);

    Task<ReportResponse> ReportAttractionAsync(ReportAttractionDto reportDto, string userId);
    Task<ReportResponse> UnreportAttractionAsync(string attractionId , string userId);
    Task<List<AttractionDto>> GetReportedAttractionsByUserAsync(string userId);






}
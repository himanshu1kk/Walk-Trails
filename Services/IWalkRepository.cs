using NzWalks.Models.Domain;
using NzWalks.Models.Dto;

namespace NzWalks{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk walk);
        Task<List<Walk>> GetAllWalkAsync(string ? filterOn=null,string?filterQuery=null,string? sortBy = null,bool isAscending = true,int pageNumber = 1,int pageSize = 1000);
        Task<Walk?> GetWalkByIdAsync(Guid id);
        Task<Walk?> UpdateWalkAsync(Guid id , Walk walk);
    }
}
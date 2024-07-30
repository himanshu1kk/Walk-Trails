using NzWalks.Models.Domain;
using NzWalks.Models.Dto;

namespace NzWalks{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk walk);
        Task<List<Walk>> GetAllWalkAsync();

        Task<Walk?> GetWalkByIdAsync(Guid id);
        Task<Walk?> UpdateWalkAsync(Guid id , Walk walk);
    }
}
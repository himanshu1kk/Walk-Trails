using NzWalks.Models.Domain;
using NzWalks.Models.Dto;

namespace NzWalks{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync();
        Task<Region?> GetByIdAsync(Guid id);

        Task<Region> CreateAsync(Region region);

        Task<Region?> UpdateAsync(Guid Id, Region region);

        Task<Region?> DeleteAsync(Guid Id);
        // Task<RegionDto> CreateAsync(RegionDto regionDomainModel);
    }
}
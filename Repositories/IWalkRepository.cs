using NzWalks.Models.Domain;
using NzWalks.Models.Dto;

namespace NzWalks{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk walk);
    }
}
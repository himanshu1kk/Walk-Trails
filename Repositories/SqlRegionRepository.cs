using Microsoft.EntityFrameworkCore;
using NzWalks.Data;
using NzWalks.Models.Domain;
using NzWalks.Models.Dto;

namespace NzWalks{

    public class SQLRegionRepository : IRegionRepository
    {
        private NzWalksDbContext dbContext;
         public SQLRegionRepository(NzWalksDbContext dbContext){
                this.dbContext = dbContext;
            }

        public async Task<Region> CreateAsync(Region region)
        {
           await  dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
        }

        public Task<RegionDto> CreateAsync(RegionDto regionDomainModel)
        {
            throw new NotImplementedException();
        }

        public async Task<Region?> DeleteAsync(Guid Id)

        {
            var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(x=>x.Id == Id);
            if(existingRegion == null){
                return null;
            }
             dbContext.Regions.Remove(existingRegion);
             await dbContext.SaveChangesAsync();
             return existingRegion;
        }

        public async Task<List<Region>> GetAllAsync()
        {
           
           return await dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return dbContext.Regions.FirstOrDefault(x => x.Id == id);
        }

        public async Task<Region> UpdateAsync(Guid Id ,Region region)
        {
            var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == region.Id);

            if(existingRegion == null) return null;
            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;    
            existingRegion.RegionImageUrl = region.RegionImageUrl;

            dbContext.Regions.Update(existingRegion);
            
            dbContext.SaveChangesAsync();
            return region;
        }
    }
}
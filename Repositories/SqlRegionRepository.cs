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

        

        public async Task<Region?> DeleteAsync(Guid Id)

        {
            var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(x=>x.Id == Id);
            if(existingRegion == null){
                return null;
            }
             dbContext.Regions.Remove(existingRegion);
             Console.WriteLine("Delete hua ki nhi");
             await dbContext.SaveChangesAsync();
                          Console.WriteLine("Delete hua ki nhi2");

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

        public async Task<Region?> UpdateAsync(Guid id, Region region)
{
    // Using the correct id parameter to find the existing region
    var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

    if (existingRegion == null)
    {
        Console.WriteLine($"Region with ID: {id} not found");
        return null;
    }

    // Updating the properties
    existingRegion.Code = region.Code;
    existingRegion.Name = region.Name;
    existingRegion.RegionImageUrl = region.RegionImageUrl;

    // Explicitly attach the entity and mark it as modified
    // dbContext.Entry(existingRegion).State = EntityState.Modified;

    // Save changes to the database
    await dbContext.SaveChangesAsync();

    return existingRegion;
}

    }
}
using Microsoft.EntityFrameworkCore;
using NzWalks.Data;
using NzWalks.Models.Domain;
using NzWalks.Models.Dto;

namespace NzWalks{

    public class SQLWalkRepository : IWalkRepository

    {
        private readonly NzWalksDbContext dbContext;

        public SQLWalkRepository(NzWalksDbContext dbContext){
            this.dbContext = dbContext;
        }
        
            

        public async Task<Walk> CreateAsync(Walk walk){

    
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;

            
        }
    }
}

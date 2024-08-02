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

        

        public async Task<List<Walk>> GetAllWalkAsync(string ? filterOn=null,string?filterQuery=null,string? sortBy = null,bool isAscending = true,int pageNumber = 1,int pageSize = 100){
            var walks = dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();
            //filtering
            if(string.IsNullOrWhiteSpace(filterOn)==false && string.IsNullOrWhiteSpace(filterQuery)==false){
                if(filterOn.Equals("Name",StringComparison.OrdinalIgnoreCase)){
                    walks = walks.Where(x=> x.Name.Contains(filterQuery));

                }

            }
            //sorting
            if(string.IsNullOrWhiteSpace(sortBy)==false)
            {
                if(sortBy.Equals("Name",StringComparison.OrdinalIgnoreCase)){
                    walks = isAscending? walks.OrderBy(x=>x.Name): walks.OrderByDescending(x=>x.Name);
                }
                else if(sortBy.Equals("Length",StringComparison.OrdinalIgnoreCase)){
                    walks = isAscending? walks.OrderBy(x=>x.LengthInKm): walks.OrderByDescending(x=>x.LengthInKm);
            }
            }
            //pagination
            var skipResults = (pageNumber-1)* pageSize;


            return await  walks.Skip(skipResults).Take(pageSize).ToListAsync();
            
            
            // return await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
            //it will collect the difficulty from the difficulty table use the difficulty id it has and sililarly it will collect the region from the region id it has
    }

        

        //now we want to get a single walk by its id so what will be the steps
        public async Task<Walk?> GetWalkByIdAsync(Guid Id){
        // if(Id == null){
        //     return null;
        Console.WriteLine("Yha aaye kki nhi");
        // }
         return await dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(w => w.Id == Id);

    }

        public async Task<Walk?> UpdateWalkAsync(Guid id, Walk walk)
        {
        var existingWalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == walk.Id);
        if(existingWalk == null){
        //write one console here
        Console.WriteLine("Yha aaye kki nhi");
        return null;
            }
        //now we can update the walk 
        existingWalk.Name  = walk.Name;
        existingWalk.Description = walk.Description;
        existingWalk.LengthInKm = walk.LengthInKm;
        existingWalk.DifficultyId = walk.DifficultyId;
        existingWalk.RegionId = walk.RegionId;
        existingWalk.WalkImageUrl = walk.WalkImageUrl;
        dbContext.Walks.Update(existingWalk);   
        await dbContext.SaveChangesAsync();
        return walk;


        }
    }
}


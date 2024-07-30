using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NzWalks.CustomActionFilter;
using NzWalks.Data;
using NzWalks.Models.Domain;
using NzWalks.Models.Dto;
using System;
using System.Collections.Generic;

namespace NzWalks.Controllers
{
    //https://localhost:portnumber/api/regions
    [ApiController]
    [Route("api/[controller]")]
    public class RegionsController : ControllerBase
    {
        private readonly NzWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        // Constructor to inject the DbContext
        public RegionsController(NzWalksDbContext dbContext,IRegionRepository regionRepository,IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        // We need to create the actions method
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // previosuly we were using the 
            // dbContext.Regions.ToListAsync();
            // to get all regions from the database and return them as a list of RegionDto
            // map each region domain model to a RegionDto and return the list of RegionDtos
            //but now we using getalls
            var regionsDomain = await regionRepository.GetAllAsync();
        //     var regionsDto = new List<RegionDto>();
        // foreach(var region in regions){
        //     regionsDto.Add(new RegionDto()
        //     {
        //         Id = region.Id,
        //         Code = region.Code,
        //         Name = region.Name,
        //        RegionImageUrl = region.RegionImageUrl
        //         }


        //     );
        // }
        //map domain to list of regiondto
       var regionsDto =  mapper.Map<List<RegionDto>>(regionsDomain);

            return Ok(regionsDto);
        }
        

        // get single region and we get region by their id
        // get //https://localhost:portnumber/api/regions/{id}

        [HttpGet]
        [Route("{id:Guid}")]
        public  async Task<IActionResult> GetById([FromRoute]Guid id){
            
        
        //    var region =  dbContext.Regions.Find(id);
        // we need to get the data from the database as a domain model and then we returns a dto to the client
        // map domain to dto and return the dto
        // var regionsDomain = await dbContext.Regions.FirstOrDefaultAsync(r => r.Id==id);
        var regionsDomain = await regionRepository.GetByIdAsync(id);
        if(regionsDomain==null){
            return NotFound();
        }
        // map region domain model to region dto
        
        return Ok(mapper.Map<RegionDto>(regionsDomain));

        

   
    }
   [HttpPost]
[ValidateModel]  // Custom attribute to validate model state
    // [Route]
    public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto) 
    {
        //map or convert dto to domain 
        var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);
        
        regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

        //map domain model to dto
        var regionDto = mapper.Map<RegionDto>(regionDomainModel); 
        Console.WriteLine(regionDto);
        // return CreatedAtAction(nameof(GetById),new {id = regionDomainModel.Id},regionDto);
        return Ok(regionDto);

    }
    
    //update region
    //put :: https://localhost:portnumber/api/regions/{id}
    [HttpPut]
    [ValidateModel] 
[Route("{id:Guid}")]
public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
{
 
    Console.WriteLine("We are inside the update");
    Console.WriteLine($"Updating region with ID: {id}");
    Console.WriteLine($"New Name: {updateRegionRequestDto.Name}");
    Console.WriteLine($"New Code: {updateRegionRequestDto.Code}");
    Console.WriteLine($"New RegionImageUrl: {updateRegionRequestDto.RegionImageUrl}");

    var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);
    var updatedRegion = await regionRepository.UpdateAsync(id, regionDomainModel);

    if (updatedRegion == null)
    {
        Console.WriteLine($"Region with ID: {id} not found");
        return NotFound();
    }

    // Convert the updated domain model to DTO
    var regionDto = mapper.Map<RegionDto>(updatedRegion);
    return Ok(regionDto);
    }
    



    //DELETE THE REGION BASED ON THE ID 
    // DELETE :: https://localhost:portnumber/api/regions/{id}
    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id){
        //we will check the id exist or not
       var regionDomainModel = await regionRepository.DeleteAsync(id);
        //remove from database'
        if(regionDomainModel==null){
            Console.WriteLine($"Region with ID: {id} not found");
            return NotFound();
        }

       var regionDto = new RegionDto{
             Id = regionDomainModel.Id,
            Code = regionDomainModel.Code,
            Name = regionDomainModel.Name,
            RegionImageUrl = regionDomainModel.RegionImageUrl
        };
        // return Ok()
        return Ok(regionDto);
       }
    
}
}



    

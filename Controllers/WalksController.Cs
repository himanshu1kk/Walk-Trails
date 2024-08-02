using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NzWalks.Data;
using NzWalks.Models.Domain;
using NzWalks.Models.Dto;
using System;
using System.Collections.Generic;

namespace NzWalks.Controllers
{
    //https://localhost:portnumber/api/walks
    [ApiController]
    [Route("api/[controller]")]
    public class WalksController: ControllerBase
    {       
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;
        public WalksController(IWalkRepository walkRepository,IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper =mapper;
        }


        //create post
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalksRequestDto addWalksRequestDto){

            //map this dto to the domain the model 
            // map add walks request dto to walsk model
            
            var walkDomainModel = mapper.Map<Walk>(addWalksRequestDto);//THIS LINE SAYS MAP THE ADD WALK REQUEST DTO TO WALK DOMAIN MODEL TAKI USME VO ID VHERAH APNE ADD HOJAYE

            

            await walkRepository.CreateAsync(walkDomainModel);
            
            //map domain model to to the dto model
           var walkDtoModel = mapper.Map<WalkDto>(walkDomainModel);
           


            return Ok(walkDtoModel);


        }
        [HttpGet]
       //get :: /API/WALKS?FILTERON=NAME&FILTERQUERY=TRACK & sortby = name & isascending = true&pageNumber = 1& pagesizw = 10
           public async Task<IActionResult> GetAllWalk([FromQuery] string? filterOn, [FromQuery] string? filterQuery,[FromQuery] string? sortBy , [FromQuery] bool? isAscending ,[FromQuery] int pageNumber = 1,[FromQuery] int pageSize = 1000){//so the concept is we can filter on the basis of colums available in the db or in the walks table so now u can decide kis basi mai filtering krni hai
            var walkDomainModel = await walkRepository.GetAllWalkAsync(filterOn,filterQuery,sortBy,isAscending??true,pageNumber,pageSize);
            //it is a domain model we need to convert it into the dto model by using mapper
            Console.WriteLine("Converting domain model to dto model for ");
            var walkDto = mapper.Map<List<WalkDto>>(walkDomainModel);
            return Ok(walkDto);



        }
        [HttpGet]
        [Route("{id:Guid}")]
        //we will pass the id to the fucntion with the route means jb hm swagger mai id dalnege to ye vali cheeze run kregi u know
        public async Task<IActionResult> GetWalkById([FromRoute] Guid id){
            //search for the id in the database if we get a null then we return a null and else we will return the domain model adn then we will convert the domain model to the dto model and will showit to user
            var walkDomainModel = await walkRepository.GetWalkByIdAsync(id);
            Console.WriteLine("Check once ");

            if(walkDomainModel == null){
                return NotFound();
            }
        // map the domain model to the dto one and then return 
        var dtoModel = mapper.Map<WalkDto>(walkDomainModel);
        return Ok(dtoModel);
        }

        //now we will have an update method for the walk part means what we will do is we need to know the id and then verify that it is the correct id and then pass a walkupdatedto model so that we can then map it to the domain model and then pass the domain model to db and in from the db we will get the domain model and then convert it into the dto model of the walk part so how we will do it
        [HttpPut]
        [Route("{id:Guid}")]

        public async Task<IActionResult> UpdateWalkById([FromRoute] Guid id,[FromBody] UpdateWalkRequestDto updateWalkRequestDto ){
            //write one console here
            Console.WriteLine("[UpdateWalk");
            var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);
            walkDomainModel =  await walkRepository.UpdateWalkAsync(id,walkDomainModel);

        //if walkdomainmodel is null return null
        if( walkDomainModel==null){
            Console.WriteLine("No walk domain model");
            return NotFound();
        }
        //map it to the dto model
        var walkDtoModel  =mapper.Map<WalkDto>(walkDomainModel);
        return Ok(walkDtoModel);

        

    }

}
}
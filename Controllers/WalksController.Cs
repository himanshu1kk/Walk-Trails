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
            Console.WriteLine("Check1 ");
            var walkDomainModel = mapper.Map<Walk>(addWalksRequestDto);
            Console.WriteLine("Check2 ");

            await walkRepository.CreateAsync(walkDomainModel);
            Console.WriteLine("Check3 ");
            //map domain model to to the dto model
           var walkDtoModel = mapper.Map<WalkDto>(walkDomainModel);
           Console.WriteLine("Check4");


            return Ok(walkDtoModel);


        }

    }

}
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NzWalks.CustomActionFilter;
using NzWalks.Data;
using NzWalks.Models.Domain;
using NzWalks.Models.Dto;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace NzWalks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;

        public AuthController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        // POST: /api/Auth/Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            Console.WriteLine("Hello Bhai1");
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };
                        Console.WriteLine("Hello Bhai2");


            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);
            if (identityResult.Succeeded)
            {
                                        Console.WriteLine("Hello Bhai3");

                // Add roles to this user
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    foreach (var role in registerRequestDto.Roles)
                    {
                        var roleResult = await userManager.AddToRoleAsync(identityUser, role);
                        if (!roleResult.Succeeded)
                        {
                            return BadRequest("Failed to add roles.");
                        }
                    }
                }
                return Ok("User was registered! Please login.");
            }
            return BadRequest("Something went wrong.");
        }
    
        //post : /api/auth/login
        [HttpPost]

        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto){
            var user = await userManager.FindByNameAsync(loginRequestDto.Username);
            if(user != null){
                var checkPasswordResult = await userManager.CheckPasswordAsync(user,loginRequestDto.Password);
                if(checkPasswordResult){
                    //succesful login hogya hai now we can assign the toke to the user
                    return Ok("User Logined Successfully");
                }
        }   
        return BadRequest("Uername or password incorrect");

    }
}
}

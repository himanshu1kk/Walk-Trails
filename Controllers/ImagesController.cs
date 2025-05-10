using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using NzWalks.CustomActionFilter;
using NzWalks.Data;
using NzWalks.Models.Domain;
using NzWalks.Models.Dto;
using System;
using System.Collections.Generic;

namespace NzWalks.Controllers
{
    //https://localhost:portnumber/api/images
    
    [Route("api/[controller]")]
    
     public class ImagesController : ControllerBase
    {
        //post method /api/images/upload
        [HttpPost]
        [Route("Upload")]//api/images upper se aaya and yha sai /upload aaya
        //we are going to pass the file in the form of a form data
                public async Task<IActionResult> Upload([FromForm] ImageUplaodRequestDto request){
                    if(ModelState.IsValid){
                        //user repository to uplaod images
                    }
                    return BadRequest(ModelState);
                }
                    //validate if the requets is correct or not
                    private void ValaidateFileUplaod(ImageUplaodRequestDto request){

                        //more then 10mb is not supported and then gve 400bad requets
                        var allowedExtension = new string[]{".jpg", ".jpeg",".png"}; 
                        if(!allowedExtension.Contains(Path.GetExtension(request.File.FileName))){   
                            ModelState.AddModelError("File", "Only .jpg,.jpeg,.png are allowed");
                        }

                        //check for the file size
                        if(request.File.Length>10485760){
                            ModelState.AddModelError("file","File size is more then 10 Mb please upload files that are less then 10mb");
                        }
                        

                    } 
                }
    }

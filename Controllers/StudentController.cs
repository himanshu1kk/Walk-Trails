using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
namespace NzWalks.Controllers{
//htpps://localhost:portnumber/api/students
 [ApiController]
[Route("api/[controller]")]

public class StudentController : ControllerBase
{   //GET:: https://localhost:portnumber/api/students
    [HttpGet]
    public IActionResult GetAllStudent(){
        // we use iaction result to return the http resposne
        string[] students = new string[] { "Himanshu","Kandpal"};
        return Ok(students);

    }
}
}
using Castle.Core.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Threading.Tasks;
using WebAppOppg2.DAL;
using WebAppOppg2.Models;
using System.Globalization;

namespace WebAppOppg2.Controllers
    //Denne filen er brukt for å teste api signaler mellom server og database ved bruk av f.eks postman
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        public SystemController()
        {
        }

        [HttpGet]
        public async Task<ActionResult> Ping()
        {
            return Ok(new
            {
                Pong = DateTime.UtcNow.ToString("s", CultureInfo.InvariantCulture)
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Greet(TestRequest test)
        {
            return Ok(new
            {
                Response = $"Hello {test.Name}, have a great day!"
            });
        }

        public class TestRequest 
        {
            public string Name { get; set;}
            public int Age {get; set;}
        }
    }
}

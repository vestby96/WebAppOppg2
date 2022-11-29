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
using static WebAppOppg2.DAL.UserRepository;

namespace WebAppOppg2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IConfiguration _config;

        private IUserRepository _db;

        private ILogger<UserController> _log;

        public UserController(IUserRepository db, ILogger<UserController> log)
        {
            _db = db;
            _log = log;
        }
       
        [HttpPost]
        public async Task<ActionResult> Register(User inUser)
        {
            if (ModelState.IsValid)
            {
                bool returOK = await _db.Register(inUser);
                if (!returOK)
                {
                    _log.LogInformation("User was NOT saved");
                    return BadRequest();
                }
                return Ok(); // kan ikke returnere noe tekst da klient prøver å konvertere deene som en Json-streng
            }
            _log.LogInformation("Error in inputvalidation");
            return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult> Authorize([FromQuery] User user) //[FromQuery] User..
        {
            if(string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
                return StatusCode(401, new { ErrorMsg = "Kunne ikke validere bruker, input mangler" });
            var userToken = await _db.LoggInn(user);
            if (string.IsNullOrEmpty(userToken))
            {
                _log.LogInformation("Innloggingen feilet for bruker" + user.Username);
                return StatusCode(401, new { ErrorMsg = "Kunne ikke validere bruker"});
            }
            return Ok(new { Token = userToken });
        }

    }
}

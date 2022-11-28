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

        private IUserRepository _db;

        private ILogger<UserController> _log;

        // usikker på om det er greit at denne er public?????????????
        public const string _loggedIn = "LoggedIn";

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

        [HttpGet("{User}")]
        public async Task<ActionResult> LoggInn(User user)
        {
            if (ModelState.IsValid)
            {
                bool returnOK = await _db.LoggInn(user);
                if (!returnOK)
                {
                    _log.LogInformation("Innloggingen feilet for bruker" + user.username);
                    HttpContext.Session.SetString(_loggedIn, "");
                    return Ok(false);
                }
                HttpContext.Session.SetString(_loggedIn, "LoggedIn");
                return Ok(true);
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest("Feil i inputvalidering på server");
        }

        public void LoggOut()
        {
            HttpContext.Session.SetString(_loggedIn, "");
        }
    }
}

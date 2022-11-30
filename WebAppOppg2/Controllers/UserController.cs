using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebAppOppg2.DAL;
using WebAppOppg2.Models;

namespace WebAppOppg2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {// her har vi mer en vanlig, en logger -log, dette må være her siden vi logger all aktivitet på server og må også sende denne inn til enhetstestene våre
        //er en del if setninger nedover, dette er feilhånteringen, hvor det også blir logget om det gikk bra eller ikke
        //vanligvis ligger det mer her, men vi har bennyttet oss av DAL som vil si at resten av formelene ligger inne på Repository og IRepository

        private readonly IUserRepository _db;

        private readonly ILogger<UserController> _log;

        public UserController(IUserRepository db, ILogger<UserController> log)
        {//log blir med her også av samme grunn som over, fordi vi trenger log for å kunne kjøre enhetstest riktig
            _db = db;
            _log = log;
        }

        [HttpPost]
        public async Task<ActionResult> Register(User inUser)
        {
            if (ModelState.IsValid)
            {
                bool returOK = await _db.Register(inUser); //sjekker om bruker ble lagret 
                if (!returOK)
                {
                    _log.LogInformation("User was NOT saved"); //dette er logging, som logger hva som gikk galt, og under returnerer en badRequest fordi ReturnOK ble feil
                    return BadRequest();
                }
                return Ok(); // kan ikke returnere noe tekst da klient prøver å konvertere deene som en Json-streng
            }
            _log.LogInformation("Error in inputvalidation"); //logging
            return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult> Authorize([FromQuery] User user) //[FromQuery] User..
        {
            //Sjekker at verken username eller password er tomme eller har verdien null
            if(string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
                return StatusCode(401, new { ErrorMsg = "Kunne ikke validere bruker, input mangler" });
            //Kaller på LoggInn matoden fra UserRepository
            var userToken = await _db.LoggInn(user);
            //sjekker at userToken ikke er tom eller har verdien null
            if (string.IsNullOrEmpty(userToken))
            {
                _log.LogInformation("Innloggingen feilet for bruker" + user.Username); //Logging
                return StatusCode(401, new { ErrorMsg = "Kunne ikke validere bruker"}); 
            }
            return Ok(new { Token = userToken });
        }

    }
}

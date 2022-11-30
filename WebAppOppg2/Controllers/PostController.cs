using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebAppOppg2.DAL;
using WebAppOppg2.Models;

namespace WebAppOppg2.Controllers
{
    [Authorize]
    [ApiController]
    
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {// her har vi mer en vanlig, en logger -log, dette må være her siden vi logger all aktivitet på server og må også sende denne inn til enhetstestene våre
        //er en del if setninger nedover, dette er feilhånteringen, hvor det også blir logget om det gikk bra eller ikke
        //vanligvis ligger det mer her, men vi har bennyttet oss av DAL som vil si at resten av formelene ligger inne på Repository og IRepository
        private IPostRepository _db;

        private ILogger<PostController> _log;

        public PostController(IPostRepository db, ILogger<PostController> log)
        {//log blir med her også av samme grunn som over, fordi vi trenger log for å kunne kjøre enhetstest riktig
            _db = db;
            _log = log;
        }

        [HttpPost]
        public async Task<ActionResult> Save(Post inPost)
        {
            if (ModelState.IsValid)
            {
                bool returOK = await _db.Save(inPost);
                if (!returOK)
                {
                    _log.LogInformation("Post was NOT saved");//dette er logging, som logger hva som gikk galt, og under returnerer en badRequest fordi ReturnOK ble feil
                    return BadRequest();
                }
                return Ok(); // kan ikke returnere noe tekst da klient prøver å konvertere deene som en Json-streng
            }
            _log.LogInformation("Error in inputvalidation");
            return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {// må lages for å kunne kjøre get all listen for å ha komunikasjon mellom frontenden(inputen) og backenden databasen nederst
            List<Post> allPosts = await _db.GetAll();
            return Ok(allPosts);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {//koblingen Delete fra knappetrykket på frontend til databasen hvor det slettes, id avgjør hvilken som blir slettet
            bool returOK = await _db.Delete(id);
            if (!returOK)
            {
                _log.LogInformation("Post was NOT deleted");//logging
                return NotFound();
            }
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetOne(int id)
        {// GetOne henter ut en post, i dette tilfellet de som blir trykket på, funnet gjennom id 
            if (ModelState.IsValid)
            {
                Post post = await _db.GetOne(id);
                if (post == null)
                {
                    _log.LogInformation("Could not fint post");//logging
                    return NotFound();
                }
                return Ok(post);
            }
            _log.LogInformation("Error in inputvalidation");//logging
            return BadRequest();
        }

        [HttpPut]
        public async Task<ActionResult> Edit(Post editPost)
        {//edit funsjonene er for å editere en post, her er koblingen mellom knappen editer, hovr man drar ut dataen fra db og legger den inn på edit siden
            if (ModelState.IsValid)
            {
                bool returOK = await _db.Edit(editPost);
                if (!returOK)
                {
                    _log.LogInformation("Edit could not be completed");//mer logging
                    return NotFound();
                }
                return Ok();
            }
            _log.LogInformation("Error in inputvalidation");
            return BadRequest();
        }
    }
}

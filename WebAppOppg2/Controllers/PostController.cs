﻿using Microsoft.AspNetCore.Http;
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
    [ApiController]
    // dekoratøren over må være med; dersom ikke må post og put bruke [FromBody] som deoratør inne i prameterlisten
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private IPostRepository _db;

        private ILogger<PostController> _log;

        public PostController(IPostRepository db, ILogger<PostController> log)
        {
            _db = db;
            _log = log;
        }

        [HttpPost]
        public async Task<ActionResult> Save(Post inPost)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(UserController._loggedIn)))
            {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                bool returOK = await _db.Save(inPost);
                if (!returOK)
                {
                    _log.LogInformation("Post was NOT saved");
                    return BadRequest();
                }
                return Ok(); // kan ikke returnere noe tekst da klient prøver å konvertere deene som en Json-streng
            }
            _log.LogInformation("Error in inputvalidation");
            return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(UserController._loggedIn)))
            {
                return Unauthorized();
            }
            List<Post> allPosts = await _db.GetAll();
            return Ok(allPosts);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(UserController._loggedIn)))
            {
                return Unauthorized();
            }
            bool returOK = await _db.Delete(id);
            if (!returOK)
            {
                _log.LogInformation("Post was NOT deleted");
                return NotFound();
            }
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetOne(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(UserController._loggedIn)))
            {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                Post post = await _db.GetOne(id);
                if (post == null)
                {
                    _log.LogInformation("Could not fint post");
                    return NotFound();
                }
                return Ok(post);
            }
            _log.LogInformation("Error in inputvalidation");
            return BadRequest();
        }

        [HttpPut]
        public async Task<ActionResult> Edit(Post editPost)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(UserController._loggedIn)))
            {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                bool returOK = await _db.Edit(editPost);
                if (!returOK)
                {
                    _log.LogInformation("Edit could not be completed");
                    return NotFound();
                }
                return Ok();
            }
            _log.LogInformation("Error in inputvalidation");
            return BadRequest();
        }
    }
}

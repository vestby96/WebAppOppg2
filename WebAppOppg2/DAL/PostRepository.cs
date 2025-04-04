﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WebAppOppg2.Models;

namespace WebAppOppg2.DAL
{//dette lå tidligere i controllersiden men ved å følge DAL så blir det mer oversiktlig og pent og derfor ligger det her
    public class PostRepository : IPostRepository
    {
        private readonly DatabaseContext _db;

        public PostRepository(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<bool> Save(Post inPost)
        {//dette er innlegging av ny foruminnlegg, hvor masse data blir flyttet til databasen. er også en catch hvis noe skulle gå feil
            try
            {
                var newPost = new Post();
                newPost.datePosted = inPost.datePosted;
                newPost.dateOccured = inPost.dateOccured;
                newPost.country = inPost.country;
                newPost.city = inPost.city;
                newPost.address = inPost.address;
                newPost.shape = inPost.shape;
                newPost.summary = inPost.summary;

                _db.Posts.Add(newPost);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task<List<Post>> GetAll()
        {//get all henter linjene i databasen til Post
            try
            {
                List<Post> allPosts = await _db.Posts.Select(p => new Post
                {
                    id = p.id,
                    datePosted = p.datePosted,
                    dateOccured = p.dateOccured,
                    country = p.country,
                    city = p.city,
                    address = p.address,
                    shape = p.shape,
                    summary = p.summary
                }).ToListAsync();
                return allPosts;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> Delete(int id)
        {//sletter en(id) utifra hvilken rad man trykker på
            try
            {
                Post aDBPost = await _db.Posts.FindAsync(id);
                _db.Posts.Remove(aDBPost);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Post> GetOne(int id)
        {// henter ut en, id bestemmer hvilken, som også blir aktivert ved å trykke på en knapp
            Post aPost = await _db.Posts.FindAsync(id);
            var getPost = new Post()
            {
                id = aPost.id,
                datePosted = aPost.datePosted,
                dateOccured = aPost.dateOccured,
                country = aPost.country,
                city = aPost.city,
                address = aPost.address,
                shape = aPost.shape,
                summary = aPost.summary
            };
            return getPost;
        }

        public async Task<bool> Edit(Post editPost)
        {//denne editerer en eksisterende item i en database
            try
            {
                var editObject = await _db.Posts.FindAsync(editPost.id);
                editObject.datePosted = editPost.datePosted;
                editObject.dateOccured = editPost.dateOccured;
                editObject.country = editPost.country;
                editObject.city = editPost.city;
                editObject.address = editPost.address;
                editObject.shape = editPost.shape;
                editObject.summary = editPost.summary;
                await _db.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}

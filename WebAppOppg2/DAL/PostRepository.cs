using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WebAppOppg2.Models;

namespace WebAppOppg2.DAL
{
    public class PostRepository : IPostRepository
    {
        private readonly PostContext _db;

        public PostRepository(PostContext db)
        {
            _db = db;
        }

        public async Task<bool> Save(Post inPost)
        {
            try
            {
                var newPost = new Posts();
                //newPost.Id = inPost.Id;
                //newPost.DatePosted = inPost.DatePosted;
                //newPost.DateOccured = inPost.DateOccured;
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
        {
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
        {
            try
            {
                Posts aDBPost = await _db.Posts.FindAsync(id);
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
        {
            Posts aPost = await _db.Posts.FindAsync(id);
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
        {
            try
            {
                var editObject = await _db.Posts.FindAsync(editPost.id);
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

        //public async Task<bool> Register(User user)
        //{
        //    try
        //    {
        //        var newUser = new Users();
        //        newUser.FirstName = user.FirstName;
        //        newUser.LastName = user.LastName;
        //        newUser.Username = user.Username;
        //        newUser.Password = user.Password;
        //        _db.Users.Add(newUser);
        //        await _db.SaveChangesAsync();
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
        public static byte[] MakeHash(string passord, byte[] salt)
        {
            return KeyDerivation.Pbkdf2(
                                password: passord,
                                salt: salt,
                                prf: KeyDerivationPrf.HMACSHA512,
                                iterationCount: 1000,
            numBytesRequested: 32);
        }

        public static byte[] MakeSalt()
        {
            var csp = new RNGCryptoServiceProvider();
            var salt = new byte[24];
            csp.GetBytes(salt);
            return salt;
        }

        public async Task<bool> LoggInn(User user)
        {
            try
            {
                Users funnetBruker = await _db.Users.FirstOrDefaultAsync(b => b.Username == user.Username);
                // sjekk passordet
                byte[] hash = MakeHash(user.Password, funnetBruker.Salt);
                bool ok = hash.SequenceEqual(funnetBruker.Password);
                if (ok)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString()); 
                //_log.LogInformation(e.Message);
                return false;
            }
        }
    }
}

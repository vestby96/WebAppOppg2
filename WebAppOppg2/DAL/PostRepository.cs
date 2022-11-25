using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
                var newPost = new Post();
                newPost.Country = inPost.Country;
                newPost.City = inPost.City;
                newPost.Address = inPost.Address;
                newPost.Shape = inPost.Shape;
                newPost.Summary = inPost.Summary;

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
                    Id = p.Id,
                    DatePosted = p.DatePosted,
                    DateOccured = p.DateOccured,
                    Country = p.Country,
                    City = p.City,
                    Address = p.Address,
                    Shape = p.Shape,
                    Summary = p.Summary
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
        {
            Post aPost = await _db.Posts.FindAsync(id);
            var getPost = new Post()
            {
                Id = aPost.Id,
                DatePosted = aPost.DatePosted,
                DateOccured = aPost.DateOccured,
                Country = aPost.Country,
                City = aPost.City,
                Address = aPost.Address,
                Shape = aPost.Shape,
                Summary = aPost.Summary
            };
            return getPost;
        }

        public async Task<bool> Edit(Post editPost)
        {
            try
            {
                var editObject = await _db.Posts.FindAsync(editPost.Id);
                editObject.DateOccured = editPost.DateOccured;
                editObject.Country = editPost.Country;
                editObject.City = editPost.City;
                editObject.Address = editPost.Address;
                editObject.Shape = editPost.Shape;
                editObject.Summary = editPost.Summary;
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

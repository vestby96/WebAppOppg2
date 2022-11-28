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
                var newPost = new Posts();
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
    }
}

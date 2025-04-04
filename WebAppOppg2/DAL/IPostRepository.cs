﻿using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebAppOppg2.Models;

namespace WebAppOppg2.DAL
{
    public interface IPostRepository
    {//disse trengs for å kunne referer til controller, Repository og mocktestene
        Task<bool> Save(Post inPost);
        Task<List<Post>> GetAll();
        Task<bool> Delete(int id);
        Task<Post> GetOne(int id);
        Task<bool> Edit(Post editPost);
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using WebAppOppg2.Models;

namespace WebAppOppg2.DAL
{
    public static class DBInit
    {
        public static void Seed(IApplicationBuilder app)
        {
            var serviceScope = app.ApplicationServices.CreateScope();

            var db = serviceScope.ServiceProvider.GetService<PostContext>();

            // må slette og opprette databasen hver gang når den skal initieres (seed`es)
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var post1 = new Posts
            {
                DatePosted = Convert.ToDateTime("2022-11-15T13:45:30"),
                DateOccured = Convert.ToDateTime("2009-06-11T21:45:00"),
                Country = "Norge",
                City = "Oslo",
                Address = "Osloveien 82",
                Shape = "Oval",
                Summary = "Fly på himmel over mitt hode"
            };

            var post2 = new Posts
            {
                DatePosted = Convert.ToDateTime("2022-10-16T12:45:00"),
                DateOccured = Convert.ToDateTime("1995-02-05T23:45:00"),
                Country = "Norge",
                City = "Asker",
                Address = "Askerveien 90",
                Shape = "Kule",
                Summary = "Kule på himmel, med masse stjerner"
            };

            db.Posts.Add(post1);
            db.Posts.Add(post2);

            db.SaveChanges();
        }
    }
}
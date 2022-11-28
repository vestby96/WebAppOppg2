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
                datePosted = Convert.ToDateTime("2022-11-15T13:45:30"),
                dateOccured = Convert.ToDateTime("2009-06-11T21:45:00"),
                country = "Norge",
                city = "Oslo",
                address = "Osloveien 82",
                shape = "Oval",
                summary = "Fly på himmel over mitt hode"
            };

            var post2 = new Posts
            {
                datePosted = Convert.ToDateTime("2022-10-16T12:45:00"),
                dateOccured = Convert.ToDateTime("1995-02-05T23:45:00"),
                country = "Norge",
                city = "Asker",
                address = "Askerveien 90",
                shape = "Kule",
                summary = "Kule på himmel med masse stjerner"
            };

            var post3 = new Posts
            {
                datePosted = Convert.ToDateTime("2022-11-15T13:45:30"),
                dateOccured = Convert.ToDateTime("2009-06-11T21:45:00"),
                country = "Norge",
                city = "Bergen",
                address = "Osloveien 82",
                shape = "Oval",
                summary = "Fly på himmel over mitt hode"
            };

            var post4 = new Posts
            {
                datePosted = Convert.ToDateTime("2022-11-15T13:45:30"),
                dateOccured = Convert.ToDateTime("2009-06-11T21:45:00"),
                country = "Norge",
                city = "Stavanger",
                address = "Stangveien 82",
                shape = "Oval",
                summary = "Fly på himmel over mitt hode"
            };

            db.Posts.Add(post1);
            db.Posts.Add(post2);
            db.Posts.Add(post3);
            db.Posts.Add(post4);

            db.SaveChanges();
        }
    }
}
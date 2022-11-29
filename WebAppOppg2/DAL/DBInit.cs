using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using WebAppOppg2.Models;
using static WebAppOppg2.Controllers.UserController;

namespace WebAppOppg2.DAL
{
    public static class DBInit
    {
        public static void Seed(IApplicationBuilder app)
        {
            var serviceScope = app.ApplicationServices.CreateScope();

            var db = serviceScope.ServiceProvider.GetService<DatabaseContext>();

            // må slette og opprette databasen hver gang når den skal initieres (seed`es)
            try
            {
                db.Database.EnsureDeleted();
            } catch { }
            db.Database.EnsureCreated();

            var post1 = new Post
            {
                datePosted = "2022-11-15",
                dateOccured = Convert.ToDateTime("2009-06-11T21:45:00"),
                country = "Norge",
                city = "Oslo",
                address = "Osloveien 82",
                shape = "Oval",
                summary = "Fly på himmel over mitt hode"
            };

            var post2 = new Post
            {
                datePosted = "2022-10-16",
                dateOccured = Convert.ToDateTime("1995-02-05T23:45:00"),
                country = "Norge",
                city = "Asker",
                address = "Askerveien 90",
                shape = "Kule",
                summary = "Kule på himmel med masse stjerner"
            };

            var post3 = new Post
            {
                datePosted = "2022-06-30",
                dateOccured = Convert.ToDateTime("1990-01-19T00:40:00"),
                country = "Sverige",
                city = "Stockholm",
                address = "Svenskeveien 2",
                shape = "Sylinder",
                summary = "Flyvende sylinder med masse lys på meg"
            };

            var post4 = new Post
            {
                datePosted = "2022-11-15",
                dateOccured = Convert.ToDateTime("2016-08-17T06:00:00"),
                country = "Danmark",
                city = "Esbjerg",
                address = "Gåveien 33",
                shape = "Oval",
                summary = "Stort fly. Altfor stort til å være vanlig passasjerfly"
            };

            //Lag en påloggingsbruker
            var user = new User();
            user.FirstName = "John";
            user.LastName = "Doe";
            user.Username = "admin";
            string password = "admin123";
            byte[] salt = UserRepository.MakeSalt();
            byte[] hash = UserRepository.MakeHash(password, salt);
            user.PasswordHashed = hash;
            user.Salt = salt;

            db.Users.Add(user);
            db.Posts.Add(post1);
            db.Posts.Add(post2);
            db.Posts.Add(post3);
            db.Posts.Add(post4);
            db.SaveChanges();
        }
    }
}
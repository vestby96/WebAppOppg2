using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAppOppg2.Models;

namespace WebAppOppg2.DAL
{
    public class Posts
    {
        public int id { get; set; }  // gir en primærnøkkel med autoincrement fordi attributten heter noe med "id"
        public DateTime datePosted { get; set; }
        public DateTime dateOccured { get; set; }
        public string country { get; set; }
        public string city { get; set; }
        public string address { get; set; }
        public string shape { get; set; }
        public string summary { get; set; }
    }

    public class Users
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public Guid? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
    }

    public class PostContext : DbContext
    {
        public PostContext(DbContextOptions<PostContext> options)
                : base(options)
        {
            // setningen under brukes for å opprette databasen fysisk dersom den ikke er opprettet
            // dette er uavhenig av initiering av databasen, se DBInit(seed)
            // når man endrer på strukturen på KundeContext er det fornuftlig å slette "Kunde.Db" fysisk før nye kjøringer
            Database.EnsureCreated();
        }

        public DbSet<Posts> Posts { get; set; }

        public DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // må importere pakken Microsoft.EntityFrameworkCore.Proxies
            // og legge til"viritual" på de attriuttene som ønskes å lastes automatisk (LazyLoading)
            optionsBuilder.UseLazyLoadingProxies();
        }

    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppOppg2.DAL
{
    public class Posts
    {
        public int Id { get; set; }  // gir en primærnøkkel med autoincrement fordi attributten heter noe med "id"
        public DateTime DatePosted { get; set; }
        public DateTime DateOccured { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Shape { get; set; }
        public string Summary { get; set; }
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // må importere pakken Microsoft.EntityFrameworkCore.Proxies
            // og legge til"viritual" på de attriuttene som ønskes å lastes automatisk (LazyLoading)
            optionsBuilder.UseLazyLoadingProxies();
        }

    }
}

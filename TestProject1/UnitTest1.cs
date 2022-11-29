using System;
using Xunit;
using Moq;
using WebAppOppg2.Controllers;
using WebAppOppg2.DAL;
using WebAppOppg2.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Net;

namespace TestProject3
{//under er alle testene våre, mer spesifik først testene fra Post(forumdatabasen med informasjon rundt ufo oppsettet), deretter testene for login
    // problemer vi møtte på her er at kilden vi har brukt, forelseningene fra læreren ikke brukte ActionResult men bool, som gjorde at vi måtte gjøre om litt
    public class UnitTest1
    {
        [Fact]
        public async Task GetAllOki()
        {//GetAllOki tester om vi klarer å legge inn 3 testbrukere, hvor vi mocker kallet til reposortyet og ser om vi klarer å kjøre controller riktig og få ut Ok(). altså vellykket kjøring
            //Arrange starter her
            var post1 = new Post
            {// under er de forskjellige mock vairablene for som blir sent til databasen
                id = 999,
                datePosted = "2021-09-06",
                dateOccured = Convert.ToDateTime("1970-01-01T01:00:00"),
                city = "oslo",
                country = "norge",
                shape = "round",
                summary = "så en rund kladd i himmelen med lys på"
            };
            var post2 = new Post
            {
                id = 998,
                datePosted = "2021-11-04",
                dateOccured = Convert.ToDateTime("1984-05-19T03:50:00"),
                city = "bergen",
                country = "norge",
                shape = "plate",
                summary = "gikk en tur med kona og så en stor talerken flygende gjennom himmelen den lyste opp hele byen men ingen andre så det"

            };
            var post3 = new Post
            {
                id = 997,
                datePosted = "2021-03-09",
                dateOccured = Convert.ToDateTime("1991-03-07T07:35:00"),
                city = "halden",
                country = "norge",
                shape = "plate",
                summary = "en talerken fløy gjennom himmelen"
            };
            var postListe = new List<Post>();//laging av liste og add post(n) til listen
            postListe.Add(post1);
            postListe.Add(post2);
            postListe.Add(post3);
            // lager to objekter for å simulere kaller repositoryet  
            var mockLogger = new Mock<ILogger<PostController>>();
            var mockPost = new Mock<IPostRepository>();
            //her begynner testen hvor vi bruker de simulerte mockene over og, kalle listen og returnerer postliste
            mockPost.Setup(p => p.GetAll()).ReturnsAsync(postListe);
            // vi setter opp repository med mockpost og mocklogger
            var postController = new PostController(mockPost.Object, mockLogger.Object);
            //act starter her, denne får det som blir retunert inn hit gjennom repositoryet da mer spesifikt gjennom postcontroller sin GetAll funsjon
            var resultat = await postController.GetAll();
            var fromController = resultat.Result as OkObjectResult;
            var res = fromController.Value;

            //assert sjekker om betingelsene vi har satt er nådd, som i dette tilfeltet er om postliste og res er like 
            Assert.Equal(postListe, res);
        }
        [Fact]
        public async Task GetAllNotOki()//teste om alle er tomme 
        {
            //Arrange starter her
            var postliste = new List<Post>();
            // lager to objekter for å simulere kaller repositoryet  
            var mockLogger = new Mock<ILogger<PostController>>();
            var mockPost = new List<IPostRepository>();
            //her begynner testen hvor vi bruker de simulerte mockene over og returner null
            mockPost.Setup(p => p.GetAll()).ReturnsAsync(() => null);
            // vi setter opp repository med mockpost og mocklogger
            var postController = new PostController(mockPost.Object, mockLogger.Object);
            List<Post> resultat = await postController.GetAll();

            //assert sjekker om resultat er true i dette tilfellet
            Assert.True(resultat);
        }

        [Fact]
        public async Task SaveOki()//tester om save ble godkjent/gjennomført riktig ved bruk av testdata
        {//lager en mock variabel som skal brukes
            //Arrange starter her
            var innforuminnlegg = new Post
            {
                id = 999,
                datePosted = "2021-09-06",
                dateOccured = Convert.ToDateTime("1970-01-01T01:00:00"),
                city = "oslo",
                country = "norge",
                shape = "round",
                summary = "så en rund kladd i himmelen med lys på"
            };
            // lager to objekter for å simulere kaller repositoryet  
            var mockLogger = new Mock<ILogger<PostController>>();
            var mockPost = new Mock<IPostRepository>();
            //her begynner testen hvor vi bruker de simulerte mockene over og, sender innforuminnlegg og returnerer true
            mockPost.Setup(p => p.Save(innforuminnlegg)).ReturnsAsync(true);
            // vi setter opp repository med mockpost og mocklogger
            var postController = new PostController(mockPost.Object, mockLogger.Object);

            var resultat = await postController.Save(innforuminnlegg);
            //assert sjekker om ok blir returnert til oss
            Assert.IsType<OkResult>(resultat);
        }

        [Fact]
        public async Task SaveNotOki()//sjekker motsatt av over om den ikke ble godkjent
        {//lager en mock variabel som skal brukes
            //Arrange starter her
            var innforuminnlegg = new Post
            {
                id = 999,
                datePosted = "2021-09-06",
                dateOccured = Convert.ToDateTime("1970-01-01T01:00:00"),
                city = "oslo",
                country = "norge",
                shape = "round",
                summary = "så en rund kladd i himmelen med lys på"
            };
            // lager to objekter for å simulere kaller repositoryet  
            var mockLogger = new Mock<ILogger<PostController>>();
            var mockPost = new Mock<IPostRepository>();
            //her begynner testen hvor vi bruker de simulerte mockene over og, hvor vi kaller innforuminnlegg og returnerer false
            mockPost.Setup(p => p.Save(innforuminnlegg)).ReturnsAsync(false);
            // vi setter opp repository med mockpost og mocklogger
            var postController = new PostController(mockPost.Object, mockLogger.Object);

            var resultat = await postController.Save(innforuminnlegg);
            //assert sjekker om vi fikk et badrequest resultat(returnert til oss)
            Assert.IsType<BadRequestResult>(resultat);
        }

        [Fact]
        public async Task GetOneOki()// sjekker om get one klarte å kjøres riktig ved bruk av mock data
        {//lager en mock variabel som skal brukes
            //Arrange starter her
            var returnePost = new Post
            {
                id = 1,
                datePosted = "2021-09-06",
                dateOccured = Convert.ToDateTime("1970-01-01T01:00:00"),
                city = "oslo",
                country = "norge",
                shape = "round",
                summary = "så en rund kladd i himmelen med lys på"
            };
            // lager to objekter for å simulere kaller repositoryet  
            var mockLogger = new Mock<ILogger<PostController>>();
            var mockPost = new Mock<IPostRepository>();
            //her begynner testen hvor vi bruker de simulerte mockene over og, hvor vi kaller getOne og returnerer returnepost
            mockPost.Setup(p => p.GetOne(1)).ReturnsAsync(returnePost);
            // vi setter opp repository med mockpost og mocklogger
            var PostController = new PostController(mockPost.Object, mockLogger.Object);
            var resultat = await PostController.GetOne(1);
            Assert.Equal<Post>(returnePost, resultat);

            List<Post> resultat = await postController.GetAll();
            //assert sjekker om vi fikk true
            Assert.True(resultat);
        }
        [Fact]
        public async Task GetOneNotOki()//motsatt av over
        {//Arrange starter her
            // lager to objekter for å simulere kaller repositoryet  
            var mockLogger = new Mock<ILogger<PostController>>();
            var mockPost = new Mock<IPostRepository>();
            //her begynner testen hvor vi bruker de simulerte mockene over og, hvor vi kaller getOne og returnerer null
            mockPost.Setup(p => p.GetOne(1)).ReturnsAsync(() => null);
            // vi setter opp repository med mockpost og mocklogger
            var postController = new PostController(mockPost.Object, mockLogger.Object);
            var resultat = await postController.GetOne(1);
            //assert sjekker om resultat er null
            Assert.Null(resultat);
        }

        [Fact]
        public async Task DeleteOki()//sjekker om det gikk an å slette i mock
        {//Arrange starter her
            // lager to objekter for å simulere kaller repositoryet  
            var mockLogger = new Mock<ILogger<PostController>>();
            var mockPost = new Mock<IPostRepository>();
            //her begynner testen hvor vi bruker de simulerte mockene over og, hvor vi kaller delete 1 og returnerer true
            mockPost.Setup(p => p.Delete(1)).ReturnsAsync(true);
            // vi setter opp repository med mockpost og mocklogger
            var postController = new PostController(mockPost.Object, mockLogger.Object);
            var resultat = await postController.Delete(1);
            //assert sjekker om vi fikk ok returnert ok() mer spesifikt
            Assert.IsType<OkResult>(resultat);
        }
        [Fact]
        public async Task DeleteNotOki()//sjekker om delete ikke funket
        {//Arrange starter her
            // lager to objekter for å simulere kaller repositoryet  
            var mockLogger = new Mock<ILogger<PostController>>();
            var mockPost = new Mock<IPostRepository>();
            //her begynner testen hvor vi bruker de simulerte mockene over og, hvor vi kaller delete 1 og returnerer false
            mockPost.Setup(p => p.Delete(1)).ReturnsAsync(false);
            // vi setter opp repository med mockpost og mocklogger
            var postController = new PostController(mockPost.Object, mockLogger.Object);
            var resultat = await postController.Delete(1);
            //assert sjekker om vi fikk 404 feil ved istype typen blank resultat 
            Assert.IsType<NotFoundResult>(resultat);
        }

        //fact er et faktum vi skal teste. altså definere et faktum, hva som skal skje
        [Fact]
        public async Task EditOki()//sjekker om edit funket riktig
        //Arrange starter her
        {//lager en mock variabel som skal brukes
            var innforuminnlegg = new Post
            {
                id = 999,
                datePosted = "2021-09-06",
                dateOccured = Convert.ToDateTime("1970-01-01T01:00:00"),
                city = "oslo",
                country = "norge",
                shape = "round",
                summary = "så en rund kladd i himmelen med lys på"
            };
            // lager to objekter for å simulere kaller repositoryet  
            var mockLogger = new Mock<ILogger<PostController>>();
            var mockPost = new Mock<IPostRepository>();
            //her begynner testen hvor vi bruker de simulerte mockene over og, hvor vi kaller innforuminnlegg og reurner true
            mockPost.Setup(p => p.Edit(innforuminnlegg)).ReturnsAsync(true);
            // vi setter opp repository med mockpost og mocklogger
            var postController = new PostController(mockPost.Object, mockLogger.Object);
            bool resultat = await PostController.Edit(innforuminnlegg);
            //assert sjekker om true blir returnert
            Assert.True(resultat);
        }

        [Fact]
        public async Task EditNotOki()//sjekker motsatt av over
        {//lager en mock variabel som skal brukes
            //Arrange starter her
            var innforuminnlegg = new Post
            {
                id = 1,
                datePosted = "2021-09-06",
                dateOccured = Convert.ToDateTime("1970-01-01T01:00:00"),
                city = "oslo",
                country = "norge",
                shape = "round",
                summary = "så en rund kladd i himmelen med lys på"
            };
            // lager to objekter for å simulere kaller repositoryet  
            var mockLogger = new Mock<ILogger<PostController>>();
            var mockPost = new Mock<IPostRepository>();
            //her begynner testen hvor vi bruker de simulerte mockene over og, hvor vi kaller innforuminnlegg og returner true
            mockPost.Setup(p => p.Edit(innforuminnlegg)).ReturnsAsync(true);
            // vi setter opp repository med mockpost og mocklogger
            var postController = new PostController(mockPost.Object, mockLogger.Object);
            bool resultat = await PostController.Edit(innforuminnlegg);
            //assert sjekker om vi fikk flase tilbake
            Assert.False(resultat);
        }
    }

}





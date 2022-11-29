using System;
using Xunit;
using Moq; // Må legge til pakken Moq.EntityFreamworkCore
using WebAppOppg2.Controllers; // må legge til en prosjektreferanse i Project-> Add Reference -> Project
using WebAppOppg2.DAL;
using WebAppOppg2.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Net;

namespace TestProject2
{

    public class UnitTest
    {
        [Fact]
        public async Task GetAllOki()
        {
            var post1 = new Post
            {
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
            var postListe = new List<Post>();
            postListe.Add(post1);
            postListe.Add(post2);
            postListe.Add(post3);

            var mockLogger = new Mock<ILogger<PostController>>();
            var mockPost = new Mock<IPostRepository>();
            mockPost.Setup(p => p.GetAll()).ReturnsAsync(postListe);
            var postController = new PostController(mockPost.Object, mockLogger.Object);

            var resultat = await postController.GetAll();
            var fromController = resultat.Result as OkObjectResult;
            var res = fromController.Value;
            Assert.Equal(postListe, res);
        }/*
        [Fact]
        public async Task GetAllNotOki()//teste om alle er tomme 
        {
            var postliste = new List<Post>();

            var mockLogger = new Mock<ILogger<PostController>>();
            var mockPost = new List<IPostRepository>();
            mockPost.Setup(p => p.GetAll()).ReturnsAsync(() => null);
            var postController = new PostController(mockPost.Object, mockLogger.Object);
            List<Post> resultat = await postController.GetAll();
            Assert.True(resultat);
        }*/

        [Fact]
        public async Task SaveOki()
        {
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
            var mockLogger = new Mock<ILogger<PostController>>();
            var mockPost = new Mock<IPostRepository>();
            mockPost.Setup(p => p.Save(innforuminnlegg)).ReturnsAsync(true);
            var postController = new PostController(mockPost.Object, mockLogger.Object);

            var resultat = await postController.Save(innforuminnlegg);
            Assert.IsType<OkResult>(resultat);
        }

        [Fact]
        public async Task SaveNotOki()
        {
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
            var mockLogger = new Mock<ILogger<PostController>>();
            var mockPost = new Mock<IPostRepository>();
            mockPost.Setup(p => p.Save(innforuminnlegg)).ReturnsAsync(false);
            var postController = new PostController(mockPost.Object, mockLogger.Object);

            var resultat = await postController.Save(innforuminnlegg);
            Assert.IsType<BadRequestResult>(resultat);
        }
        /*
        [Fact]
        public async Task GetOneOki()
        {
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
            var mock = new Mock<IPostRepository>();
            mock.Setup(p => p.GetOne(1)).ReturnsAsync(returnePost);
            var PostController = new PostController(mock.Object);
            Post resultat = await PostController.GetOne(1);
            Assert.Equal<Post>(returnePost, resultat);
        }
        [Fact]
        public async Task GetOneNotOki()
        {
            var mock = new Mock<IPostRepository>();
            mock.setup(p => p.GetOne(1)).ReturnsAsync(() => null);
            var postController = new PostController(mock.Object);
            Post resultat = await postController.GetOne(1);
            Assert.Null(resultat);
        }
        */
        [Fact]
        public async Task DeleteOki()
        {
            var mockLogger = new Mock<ILogger<PostController>>();
            var mockPost = new Mock<IPostRepository>();
            mockPost.Setup(p => p.Delete(1)).ReturnsAsync(true);
            var postController = new PostController(mockPost.Object, mockLogger.Object);
            var resultat = await postController.Delete(1);
            Assert.IsType<OkResult>(resultat);
        }
        [Fact]
        public async Task DeleteNotOki()
        {
            var mockLogger = new Mock<ILogger<PostController>>();
            var mockPost = new Mock<IPostRepository>();
            mockPost.Setup(p => p.Delete(1)).ReturnsAsync(false);
            var postController = new PostController(mockPost.Object, mockLogger.Object);
            var resultat = await postController.Delete(1);
            Assert.IsType<NotFoundResult>(resultat);
        }
        /*
        //fact er et faktum vi skal teste. altså definere et faktum, hva som skal skje
        [Fact]
        public async Task EditOki()
        {
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

            var mock = new Mock<IPostRepository>();
            mock.Setup(p => p.Edit(innforuminnlegg)).ReturnsAsync(true);
            var postController = new PostController(mock.Object);
            bool resultat = await PostController.Edit(innforuminnlegg);
            Assert.True(resultat);
        }

        [Fact]
        public async Task EditNotOki()
        {

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

            var mock = new Mock<IPostRepository>();
            mock.Setup(p => p.Edit(innforuminnlegg)).ReturnsAsync(true);
            var postController = new PostController(mock.Object);
            bool resultat = await PostController.Edit(innforuminnlegg);
            Assert.False(resultat);
        }*/
    }

}






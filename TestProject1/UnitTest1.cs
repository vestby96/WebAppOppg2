using System;
using Xunit;
using Moq; // Må legge til pakken Moq.EntityFreamworkCore
using WebAppOppg2.Controllers; // må legge til en prosjektreferanse i Project-> Add Reference -> Project
using WebAppOppg2.DAL;
using WebAppOppg2.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;

namespace TestProject2
{

    public class PostTester
    {
        [Fact]
        public async Task GetAll()
        {
            var forum1 = new Post
            {
                city = "oslo",
                country = "norge",
                shape = "round",
                summary = "så en rund kladd i himmelen med lys på"

            };
            var forum2 = new Post
            {
                city = "bergen",
                country = "norge",
                shape = "plate",
                summary = "gikk en tur med kona og så en stor talerken flygende gjennom himmelen den lyste opp hele byen men ingen andre så det"

            };
            var forum3 = new Post
            {
                city = "halden",
                country = "norge",
                shape = "plate",
                summary = "en talerken fløy gjennom himmelen"

            };
            var postListe = new List<Post>();
            postListe.Add(forum1);
            postListe.Add(forum2);
            postListe.Add(forum3);

            var mock = new Mock<IPostRepository>();
            mock.Setup(p => p.GetAll()).ReturnsAsync(postListe);
            var PostController = new PostController(mock.Object);
            List<Post> resultat = await PostController.GetAll();
            Assert.Equal<List<Post>>(postListe, resultat);


        }
        [Fact]
        public async Task GetAllEmpty()//teste om alle er tomme 
        {
            var postliste = new List<Post>();

            var mock = new List<IPostRepository>();
            mock.Setup(p => p.GetAll()).ReturnsAsync(() => null);
            var PostController = new PostController(mock.Object);
            List<Post> resultat = await postController.GetAll();
            Assert.True(resultat);
        }
        [Fact]
        public async Task SaveOki()
        {
            var InPost = new Post
            {
                city = "oslo",
                country = "norge",
                shape = "round",
                summary = "så en rund kladd i himmelen med lys på"

            };
            var mock = new Mock<IPostRepository>();
            mock.Setup(p => p.Save(InPost)).ReturnsAsync(true);
            var postController = new PostController(mock.Object);
            bool resultat = await postController.Save(InPost);
            Assert.True(resultat);


        }


        [Fact]
        public async Task SaveNotOki()
        {
            var InPost = new Post
            {
                city = "oslo",
                country = "norge",
                shape = "round",
                summary = "så en rund kladd i himmelen med lys på"

            };
            var mock = new Mock<IPostRepository>();
            mock.Setup(p => p.save(InPost)).ReturnsAsync(false);
            var postController = new PostController(mock.Object);
            Post resultat = await postController.Save(InPost);
            Assert.False(resultat);

        }
        [Fact]
        public async Task GetOneOki()
        {
            var returnePost = new Post
            {
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

        [Fact]
        public async Task DeleteOki()
        {


            var mock = new Mock<IPostRepository>();
            mock.Setup(p => p.Delete(1)).ReturnsAsync(true);
            var PostController = new PostController(mock.Object);
            bool resultat = await PostController.Delete(1);
            Assert.True(resultat);

        }
        [Fact]
        public async Task DeleteNotOki()
        {
            var mock = new Mock<IPostRepository>();
            mock.setup(p => p.Delete(1)).ReturnsAsync(false);
            var postController = new PostController(mock.Object);
            bool resultat = await postController.Delete(1);
            Assert.False(resultat);

        }

        //fact er et faktum vi skal teste. altså definere et faktum, hva som skal skje
        [Fact]//denne er lik EndreOk
        public async Task Save()
        {
            //lagre et foruminnleg/eller ny bruker
            var InPost = new Post
            {

                city = "oslo",
                country = "norge",
                shape = "round",
                summary = "så en rund kladd i himmelen med lys på"

            };

            var mock = new Mock<IPostRepository>();
            mock.Setup(p => p.Save(InPost)).ReturnsAsync(true);
            var postController = new PostController(mock.Object);

            //så skal vi kalle 

            bool resultat = await postController.Save(InPost);

            //sjekke at resultatet er riktig 
            Assert.True(resultat);
        }

        [Fact]
        public async Task GetOne()
        {
            var returnePost = new Post
            {
                city = "oslo",
                country = "norge",
                shape = "round",
                summary = "så en rund kladd i himmelen med lys på"
            };
            var mock = new Mock<IPostRepository>();
            mock.Setup(p => p.GetOne(1)).ReturnsAsync(returnePost);
            var postController = new PostController.GetOne(1);
            Post resultat = await postController.GetOne(1);
            Assert.Equal<Post>(returnePost, resultat);

        }
        [Fact]
        public async Task Edit()
        {

            var InPost = new Post
            {

                city = "oslo",
                country = "norge",
                shape = "round",
                summary = "så en rund kladd i himmelen med lys på"

            };

            var mock = new Mock<IPostRepository>();
            mock.Setup(p => p.Edit(InPost)).ReturnsAsync(true);
            var postController = new PostController(mock.Object);
            bool resultat = await PostController.Edit(InPost);
            Assert.True(resultat);
        }

        [Fact]
        public async Task EditNotOki()
        {

            var InPost = new Post
            {

                city = "oslo",
                country = "norge",
                shape = "round",
                summary = "så en rund kladd i himmelen med lys på"

            };

            var mock = new Mock<IPostRepository>();
            mock.Setup(p => p.Edit(InPost)).ReturnsAsync(true);
            var postController = new PostController(mock.Object);
            bool resultat = await PostController.Edit(InPost);
            Assert.False(resultat);
        }
    }

}


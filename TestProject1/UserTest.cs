using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WebAppOppg2.Controllers;
using WebAppOppg2.DAL;
using WebAppOppg2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System.Text;
using System;

namespace KundeAppTest
{
    public class UserTest
    {
        private const string _loggetInn = "loggetInn";
        private const string _ikkeLoggetInn = "";

        private readonly Mock<IPostRepository> mockPst = new Mock<IPostRepository>();
        private readonly Mock<ILogger<PostController>> mockLog = new Mock<ILogger<PostController>>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();

        [Fact]
        public async Task GetAllLoggedInOk()
        {
            // Arrange
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

            var postlist = new List<Post>();
            postlist.Add(post1);
            postlist.Add(post2);
            postlist.Add(post3);

            mockPst.Setup(p => p.GetAll()).ReturnsAsync(postlist);

            var postController = new PostController(mockPst.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            postController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await postController.GetAll() as OkObjectResult;

            // Assert
            // definerer resultat som OkObjectResult type, og henter ut verdien
            var model = resultat.Value;
            // skjekker om verdien ut matcher verdien inn
            Assert.Equal(postlist, model);
        }

        [Fact]
        public async Task HentAlleIkkeLoggetInn()
        {
            // Arrange
            mockPst.Setup(k => k.GetAll()).ReturnsAsync(It.IsAny<List<Post>>);

            var postController = new PostController(mockPst.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            postController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await postController.GetAll() as UnauthorizedObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value.ToString());
        }

        [Fact]
        public async Task LagreLoggetInnOK()
        {
            // Arrange
            mockPst.Setup(k => k.Save(It.IsAny<Post>())).ReturnsAsync(true);

            var postController = new PostController(mockPst.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            postController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await postController.Save(It.IsAny<Post>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal("Kunde lagret", resultat.Value.ToString());
        }

        [Fact]
        public async Task LagreLoggetInnIkkeOK()
        {
            // Arrange

            mockPst.Setup(k => k.Save(It.IsAny<Post>())).ReturnsAsync(false);

            var postController = new PostController(mockPst.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            postController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await postController.Save(It.IsAny<Post>()) as BadRequestObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Kunden kunne ikke lagres", resultat.Value.ToString());
        }

        [Fact]
        public async Task LagreLoggetInnFeilModel()
        {
            // Arrange
            // Kunden er indikert feil med tomt fornavn her.
            // det har ikke noe å si, det er introduksjonen med ModelError under som tvinger frem feilen
            // kunne også her brukt It.IsAny<Post>
            var post1 = new Post
            {
                id = 999,
                datePosted = "2021-09-06",
                dateOccured = Convert.ToDateTime("1970-01-01T01:00:00"),
                city = "",
                country = "norge",
                shape = "round",
                summary = "så en rund kladd i himmelen med lys på"
            };

            mockPst.Setup(k => k.Save(post1)).ReturnsAsync(true);

            var postController = new PostController(mockPst.Object, mockLog.Object);

            postController.ModelState.AddModelError("city", "Feil i inputvalidering på server");

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            postController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await postController.Save(post1) as BadRequestObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Feil i inputvalidering på server", resultat.Value.ToString());
        }

        [Fact]
        public async Task LagreIkkeLoggetInn()
        {
            mockPst.Setup(k => k.Save(It.IsAny<Post>())).ReturnsAsync(true);

            var postController = new PostController(mockPst.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            postController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await postController.Save(It.IsAny<Post>()) as UnauthorizedObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value.ToString());
        }

        [Fact]
        public async Task SlettLoggetInnOK()
        {
            // Arrange

            mockPst.Setup(k => k.Delete(It.IsAny<int>())).ReturnsAsync(true);

            var postController = new PostController(mockPst.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            postController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await postController.Delete(It.IsAny<int>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal("Kunde slettet", resultat.Value.ToString());
        }

        [Fact]
        public async Task SlettLoggetInnIkkeOK()
        {
            // Arrange

            mockPst.Setup(k => k.Delete(It.IsAny<int>())).ReturnsAsync(false);

            var postController = new PostController(mockPst.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            postController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await postController.Delete(It.IsAny<int>()) as NotFoundObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Sletting av Kunden ble ikke utført", resultat.Value.ToString());
        }

        [Fact]
        public async Task SletteIkkeLoggetInn()
        {
            mockPst.Setup(k => k.Delete(It.IsAny<int>())).ReturnsAsync(true);

            var postController = new PostController(mockPst.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            postController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await postController.Delete(It.IsAny<int>()) as UnauthorizedObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value.ToString());
        }

        [Fact]
        public async Task HentEnLoggetInnOK()
        {
            // Arrange
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

            mockPst.Setup(k => k.GetOne(It.IsAny<int>())).ReturnsAsync(post1);

            var postController = new PostController(mockPst.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            postController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await postController.GetOne(It.IsAny<int>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal(post1, (Post)resultat.Value);
        }
        
        [Fact]
        public async Task HentEnLoggetInnIkkeOK()
        {
            // Arrange

            mockPst.Setup(k => k.GetOne(It.IsAny<int>())).ReturnsAsync(() => null); // merk denne null setting!

            var postController = new PostController(mockPst.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            postController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await postController.GetOne(It.IsAny<int>()) as NotFoundObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Fant ikke kunden", resultat.Value.ToString());
        }

        [Fact]
        public async Task HentEnIkkeLoggetInn()
        {
            mockPst.Setup(k => k.GetOne(It.IsAny<int>())).ReturnsAsync(() => null);

            var postController = new PostController(mockPst.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            postController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await postController.GetOne(It.IsAny<int>()) as UnauthorizedObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value.ToString());
        }

        [Fact]
        public async Task EndreLoggetInnOK()
        {
            // Arrange

            mockPst.Setup(k => k.Edit(It.IsAny<Post>())).ReturnsAsync(true);

            var postController = new PostController(mockPst.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            postController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await postController.Edit(It.IsAny<Post>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal("Kunde endret", resultat.Value.ToString());
        }

        [Fact]
        public async Task EndreLoggetInnIkkeOK()
        {
            // Arrange

            mockPst.Setup(k => k.Edit(It.IsAny<Post>())).ReturnsAsync(false);

            var postController = new PostController(mockPst.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            postController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await postController.Edit(It.IsAny<Post>()) as NotFoundObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Endringen av kunden kunne ikke utføres", resultat.Value.ToString());
        }

        [Fact]
        public async Task EndreLoggetInnFeilModel()
        {
            // Arrange
            // Kunden er indikert feil med tomt fornavn her.
            // det har ikke noe å si, det er introduksjonen med ModelError under som tvinger frem feilen
            // kunne også her brukt It.IsAny<Post>
            var post1 = new Post
            {
                id = 999,
                datePosted = "2021-09-06",
                dateOccured = Convert.ToDateTime("1970-01-01T01:00:00"),
                city = "",
                country = "norge",
                shape = "round",
                summary = "så en rund kladd i himmelen med lys på"
            };

            mockPst.Setup(k => k.Edit(post1)).ReturnsAsync(true);

            var postController = new PostController(mockPst.Object, mockLog.Object);

            postController.ModelState.AddModelError("city", "Feil i inputvalidering på server");

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            postController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await postController.Edit(post1) as BadRequestObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Feil i inputvalidering på server", resultat.Value.ToString());
        }

        [Fact]
        public async Task EndreIkkeLoggetInn()
        {
            mockPst.Setup(k => k.Edit(It.IsAny<Post>())).ReturnsAsync(true);

            var postController = new PostController(mockPst.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            postController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await postController.Edit(It.IsAny<Post>()) as UnauthorizedObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value.ToString());
        }

        // ------------- Her begynner User testene ----------------------------------------

        private readonly Mock<IUserRepository> mockUsr = new Mock<IUserRepository>();
        private readonly Mock<ILogger<UserController>> mockLogU = new Mock<ILogger<UserController>>();

        [Fact]
        public async Task LoggInnOK()
        {
            mockUsr.Setup(k => k.LoggInn(It.IsAny<User>())).ReturnsAsync(true);

            var userController = new UserController(mockUsr.Object, mockLogU.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            userController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await userController.LoggInn(It.IsAny<User>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.True((bool)resultat.Value);
        }

        [Fact]
        public async Task LoggInnFeilPassordEllerBruker()
        {
            mockUsr.Setup(k => k.LoggInn(It.IsAny<User>())).ReturnsAsync(false);

            var userController = new UserController(mockUsr.Object, mockLogU.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            userController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await userController.LoggInn(It.IsAny<User>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.False((bool)resultat.Value);
        }

        [Fact]
        public async Task LoggInnInputFeil()
        {
            mockUsr.Setup(k => k.LoggInn(It.IsAny<User>())).ReturnsAsync(true);

            var userController = new UserController(mockUsr.Object, mockLogU.Object);

            userController.ModelState.AddModelError("Brukernavn", "Feil i inputvalidering på server");

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            userController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await userController.LoggInn(It.IsAny<User>()) as BadRequestObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Feil i inputvalidering på server", resultat.Value.ToString());
        }

        [Fact]
        public void LoggUt()
        {
            var userController = new UserController(mockUsr.Object, mockLogU.Object);

            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            mockSession[_loggetInn] = _loggetInn;
            userController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            userController.LoggUt();

            // Assert
            Assert.Equal(_ikkeLoggetInn, mockSession[_loggetInn]);
        }
    }
}

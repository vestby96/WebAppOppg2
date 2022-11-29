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

namespace KundeAppTest
{
    public class KundeAppTest
    {
        private const string _loggetInn = "loggetInn";
        private const string _ikkeLoggetInn = "";

        private readonly Mock<IUserRepository> mockRep = new Mock<IUserRepository>();
        private readonly Mock<ILogger<UserRepository>> mockLog = new Mock<ILogger<UserRepository>>();

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();

        [Fact]
        public async Task HentAlleLoggetInnOK()
        {

            // Arrange
            var user1 = new User
            {
                Id = 100,
                FirstName = "Ola",
                LastName = "Normann",
                Username = "OlaNormann"
                 
            };
            var user2 = new User
            {
                Id = 101,
                FirstName = "Ole",
                LastName = "Normann",
                Username = "OleNormann"

            };
            var user3 = new User
            {
                Id = 102,
                FirstName = "Olga",
                LastName = "Normann",
                Username = "OlgaNormann"

            };

            var userliste = new List<User>();
            userliste.Add(user1);
            userliste.Add(user2);
            userliste.Add(user3);

            mockRep.Setup(k => k.GetAll()).ReturnsAsync(userliste);

            var kundeController = new UserController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.GetAll() as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal<List<Kunde>>((List<Kunde>)resultat.Value, kundeListe);
        }

        [Fact]
        public async Task HentAlleIkkeLoggetInn()
        {
            // Arrange

            //var tomListe = new List<Kunde>();

            mockRep.Setup(k => k.HentAlle()).ReturnsAsync(It.IsAny<List<Kunde>>());

            var kundeController = new KundeController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.HentAlle() as UnauthorizedObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value);
        }

        [Fact]
        public async Task LagreLoggetInnOK()
        {

            /*  Kan unngå denne med It.IsAny<Kunde>
            var kunde1 = new Kunde {Id = 1,Fornavn = "Per",Etternavn = "Hansen",Adresse = "Askerveien 82",
                                    Postnr = "1370",Poststed = "Asker"};
            */

            // Arrange

            mockRep.Setup(k => k.Lagre(It.IsAny<Kunde>())).ReturnsAsync(true);

            var kundeController = new KundeController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.Lagre(It.IsAny<Kunde>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal("Kunde lagret", resultat.Value);
        }

        [Fact]
        public async Task LagreLoggetInnIkkeOK()
        {
            // Arrange

            mockRep.Setup(k => k.Lagre(It.IsAny<Kunde>())).ReturnsAsync(false);

            var kundeController = new KundeController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.Lagre(It.IsAny<Kunde>()) as BadRequestObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Kunden kunne ikke lagres", resultat.Value);
        }

        [Fact]
        public async Task LagreLoggetInnFeilModel()
        {
            // Arrange
            // Kunden er indikert feil med tomt fornavn her.
            // det har ikke noe å si, det er introduksjonen med ModelError under som tvinger frem feilen
            // kunnde også her brukt It.IsAny<Kunde>
            var kunde1 = new Kunde
            {
                Id = 1,
                Fornavn = "",
                Etternavn = "Hansen",
                Adresse = "Askerveien 82",
                Postnr = "1370",
                Poststed = "Asker"
            };

            mockRep.Setup(k => k.Lagre(kunde1)).ReturnsAsync(true);

            var kundeController = new KundeController(mockRep.Object, mockLog.Object);

            kundeController.ModelState.AddModelError("Fornavn", "Feil i inputvalidering på server");

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.Lagre(kunde1) as BadRequestObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Feil i inputvalidering på server", resultat.Value);
        }

        [Fact]
        public async Task LagreIkkeLoggetInn()
        {
            mockRep.Setup(k => k.Lagre(It.IsAny<Kunde>())).ReturnsAsync(true);

            var kundeController = new KundeController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.Lagre(It.IsAny<Kunde>()) as UnauthorizedObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value);
        }

        [Fact]
        public async Task SlettLoggetInnOK()
        {
            // Arrange

            mockRep.Setup(k => k.Slett(It.IsAny<int>())).ReturnsAsync(true);

            var kundeController = new KundeController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.Slett(It.IsAny<int>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal("Kunde slettet", resultat.Value);
        }

        [Fact]
        public async Task SlettLoggetInnIkkeOK()
        {
            // Arrange

            mockRep.Setup(k => k.Slett(It.IsAny<int>())).ReturnsAsync(false);

            var kundeController = new KundeController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.Slett(It.IsAny<int>()) as NotFoundObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Sletting av Kunden ble ikke utført", resultat.Value);
        }

        [Fact]
        public async Task SletteIkkeLoggetInn()
        {
            mockRep.Setup(k => k.Slett(It.IsAny<int>())).ReturnsAsync(true);

            var kundeController = new KundeController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.Slett(It.IsAny<int>()) as UnauthorizedObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value);
        }

        [Fact]
        public async Task HentEnLoggetInnOK()
        {
            // Arrange
            var kunde1 = new Kunde
            {
                Id = 1,
                Fornavn = "Per",
                Etternavn = "Hansen",
                Adresse = "Askerveien 82",
                Postnr = "1370",
                Poststed = "Asker"
            };

            mockRep.Setup(k => k.HentEn(It.IsAny<int>())).ReturnsAsync(kunde1);

            var kundeController = new KundeController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.HentEn(It.IsAny<int>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal<Kunde>(kunde1, (Kunde)resultat.Value);
        }

        [Fact]
        public async Task HentEnLoggetInnIkkeOK()
        {
            // Arrange

            mockRep.Setup(k => k.HentEn(It.IsAny<int>())).ReturnsAsync(() => null); // merk denne null setting!

            var kundeController = new KundeController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.HentEn(It.IsAny<int>()) as NotFoundObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Fant ikke kunden", resultat.Value);
        }

        [Fact]
        public async Task HentEnIkkeLoggetInn()
        {
            mockRep.Setup(k => k.HentEn(It.IsAny<int>())).ReturnsAsync(() => null);

            var kundeController = new KundeController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.HentEn(It.IsAny<int>()) as UnauthorizedObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value);
        }

        [Fact]
        public async Task EndreLoggetInnOK()
        {
            // Arrange

            mockRep.Setup(k => k.Endre(It.IsAny<Kunde>())).ReturnsAsync(true);

            var kundeController = new KundeController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.Endre(It.IsAny<Kunde>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.Equal("Kunde endret", resultat.Value);
        }

        [Fact]
        public async Task EndreLoggetInnIkkeOK()
        {
            // Arrange

            mockRep.Setup(k => k.Lagre(It.IsAny<Kunde>())).ReturnsAsync(false);

            var kundeController = new KundeController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.Endre(It.IsAny<Kunde>()) as NotFoundObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.NotFound, resultat.StatusCode);
            Assert.Equal("Endringen av kunden kunne ikke utføres", resultat.Value);
        }

        [Fact]
        public async Task EndreLoggetInnFeilModel()
        {
            // Arrange
            // Kunden er indikert feil med tomt fornavn her.
            // det har ikke noe å si, det er introduksjonen med ModelError under som tvinger frem feilen
            // kunnde også her brukt It.IsAny<Kunde>
            var kunde1 = new Kunde
            {
                Id = 1,
                Fornavn = "",
                Etternavn = "Hansen",
                Adresse = "Askerveien 82",
                Postnr = "1370",
                Poststed = "Asker"
            };

            mockRep.Setup(k => k.Endre(kunde1)).ReturnsAsync(true);

            var kundeController = new KundeController(mockRep.Object, mockLog.Object);

            kundeController.ModelState.AddModelError("Fornavn", "Feil i inputvalidering på server");

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.Endre(kunde1) as BadRequestObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Feil i inputvalidering på server", resultat.Value);
        }

        [Fact]
        public async Task EndreIkkeLoggetInn()
        {
            mockRep.Setup(k => k.Endre(It.IsAny<Kunde>())).ReturnsAsync(true);

            var kundeController = new KundeController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.Endre(It.IsAny<Kunde>()) as UnauthorizedObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.Unauthorized, resultat.StatusCode);
            Assert.Equal("Ikke logget inn", resultat.Value);
        }

        [Fact]
        public async Task LoggInnOK()
        {
            mockRep.Setup(k => k.LoggInn(It.IsAny<Bruker>())).ReturnsAsync(true);

            var kundeController = new KundeController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.LoggInn(It.IsAny<Bruker>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.True((bool)resultat.Value);
        }

        [Fact]
        public async Task LoggInnFeilPassordEllerBruker()
        {
            mockRep.Setup(k => k.LoggInn(It.IsAny<Bruker>())).ReturnsAsync(false);

            var kundeController = new KundeController(mockRep.Object, mockLog.Object);

            mockSession[_loggetInn] = _ikkeLoggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.LoggInn(It.IsAny<Bruker>()) as OkObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.OK, resultat.StatusCode);
            Assert.False((bool)resultat.Value);
        }

        [Fact]
        public async Task LoggInnInputFeil()
        {
            mockRep.Setup(k => k.LoggInn(It.IsAny<Bruker>())).ReturnsAsync(true);

            var kundeController = new KundeController(mockRep.Object, mockLog.Object);

            kundeController.ModelState.AddModelError("Brukernavn", "Feil i inputvalidering på server");

            mockSession[_loggetInn] = _loggetInn;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var resultat = await kundeController.LoggInn(It.IsAny<Bruker>()) as BadRequestObjectResult;

            // Assert 
            Assert.Equal((int)HttpStatusCode.BadRequest, resultat.StatusCode);
            Assert.Equal("Feil i inputvalidering på server", resultat.Value);
        }

        [Fact]
        public void LoggUt()
        {
            var kundeController = new KundeController(mockRep.Object, mockLog.Object);

            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            mockSession[_loggetInn] = _loggetInn;
            kundeController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            kundeController.LoggUt();

            // Assert
            Assert.Equal(_ikkeLoggetInn, mockSession[_loggetInn]);
        }
    }
}

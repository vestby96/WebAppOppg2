using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebAppOppg2.Models;

namespace WebAppOppg2.DAL
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _db;

        public UserRepository(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<bool> Register(User user)
        {//dette er innlegging av nye brukere, hvor data blir flyttet til databasen. Vi har også en catch hvis noe skulle gå feil
            try
            {
                var newUser = new User();
                newUser.FirstName = user.FirstName;
                newUser.LastName = user.LastName;
                newUser.Username = user.Username;
                byte[] salt = UserRepository.MakeSalt(); //Kaler på MakeSalt() og lagere salt
                byte[] hash = UserRepository.MakeHash(user.Password, salt); //Kaller på MakeHash() og lagrer resultatet
                newUser.PasswordHashed = hash;
                newUser.Salt = salt;

                _db.Users.Add(newUser); //legger den nye brukeren inn i databasen
                await _db.SaveChangesAsync(); //lagrer endringene i databasen
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static byte[] MakeSalt() //Lager et salt for en bruker
        {
            var csp = new RNGCryptoServiceProvider();
            var salt = new byte[24];
            csp.GetBytes(salt);
            return salt;
        }


        public static byte[] MakeHash(string passord, byte[] salt) //Lager et hash for en bruker
        {
            return KeyDerivation.Pbkdf2(
                                password: passord,
                                salt: salt,
                                prf: KeyDerivationPrf.HMACSHA512,
                                iterationCount: 1000,
            numBytesRequested: 32);
        }
        public async Task<string> LoggInn(User user) 
        {
            try
            {
                //Sjekker om vi finner et matchende brukernavn i databasen
                User funnetBruker = await _db.Users.FirstOrDefaultAsync(b => b.Username.ToLower()==user.Username.ToLower().Trim());
                //Hvis vi ikke finner en bruker så kast en exeption
                if (funnetBruker == null) {
                    throw new Exception();
                }
                //Validate Password
                byte[] hash = MakeHash(user.Password, funnetBruker.Salt); //Lager hash basert på passord fra input og salt fra databasen
                bool passwordMatch = hash.SequenceEqual(funnetBruker.PasswordHashed); //Sjekker om nytt hash matcher hash i databasen
                if(passwordMatch is false) return string.Empty;

                var issuer = AppData.JwtIssuer; 
                var key = Encoding.ASCII.GetBytes(AppData.JwtKey);

                //Lager ny tokenDescriptor basert på bruker info
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Jti, $"{Guid.NewGuid()}"),
                        new Claim(JwtRegisteredClaimNames.Sub, funnetBruker.Username),
                        new Claim("Firstname", funnetBruker.FirstName),
                        new Claim("Lastname", funnetBruker.LastName)
                    }),
                    Expires = DateTime.UtcNow.AddDays(30), //Token er gyldig i 30 dager
                    Issuer = issuer,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
                };
                //Lager ny token for bruker
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);
                var stringToken = tokenHandler.WriteToken(token);

                return stringToken; //returnerer brukerens token
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString()); 
                return string.Empty; //hvis noe går galt retuner en tom string
            }
        }
    }
}

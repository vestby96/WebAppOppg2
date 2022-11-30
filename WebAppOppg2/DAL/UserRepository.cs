using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
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
        {
            try
            {
                var newUser = new User();
                newUser.FirstName = user.FirstName;
                newUser.LastName = user.LastName;
                newUser.Username = user.Username;
                byte[] salt = MakeSalt();
                byte[] hash = UserRepository.MakeHash(user.Password, salt);
                newUser.PasswordHashed = hash;
                newUser.Salt = salt;

                _db.Users.Add(newUser);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static byte[] MakeHash(string passord, byte[] salt)
        {
            return KeyDerivation.Pbkdf2(
                                password: passord,
                                salt: salt,
                                prf: KeyDerivationPrf.HMACSHA512,
                                iterationCount: 1000,
            numBytesRequested: 32);
        }

        public static byte[] MakeSalt()
        {
            var csp = new RNGCryptoServiceProvider();
            var salt = new byte[24];
            csp.GetBytes(salt);
            return salt;
        }

        public async Task<string> LoggInn(User user)
        {
            try
            {
                User funnetBruker = await _db.Users.FirstOrDefaultAsync(b => b.Username.ToLower() == user.Username.ToLower().Trim());
                //Validate Password
                byte[] hash = MakeHash(user.Password, funnetBruker.Salt);
                bool passwordMatch = hash.SequenceEqual(funnetBruker.PasswordHashed);
                if (passwordMatch is false) return string.Empty;

                var issuer = AppData.JwtIssuer;
                var key = Encoding.ASCII.GetBytes(AppData.JwtKey);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Jti, $"{Guid.NewGuid()}"),
                        new Claim(JwtRegisteredClaimNames.Sub, funnetBruker.Username),
                        new Claim("Firstname", funnetBruker.FirstName),
                        new Claim("Lastname", funnetBruker.LastName)
                    }),
                    Expires = DateTime.UtcNow.AddDays(30),
                    Issuer = issuer,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);
                var stringToken = tokenHandler.WriteToken(token);

                return stringToken;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                //_log.LogInformation(e.Message);
                return string.Empty;
            }
        }
    }
}

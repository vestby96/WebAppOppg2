using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
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
                newUser.firstName = user.firstName;
                newUser.lastName = user.lastName;
                newUser.username = user.username;
                byte[] salt = UserRepository.MakeSalt();
                byte[] hash = UserRepository.MakeHash(user.password, salt);
                newUser.passwordHashed = hash;
                newUser.salt = salt;

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

        public async Task<bool> LoggInn(User user)
        {
            try
            {
                User funnetBruker = await _db.Users.FirstOrDefaultAsync(b => b.username == user.username);
                // sjekk passordet
                byte[] hash = MakeHash(user.password, funnetBruker.salt);
                bool ok = hash.SequenceEqual(funnetBruker.passwordHashed);
                if (ok)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString()); 
                //_log.LogInformation(e.Message);
                return false;
            }
        }
    }
}

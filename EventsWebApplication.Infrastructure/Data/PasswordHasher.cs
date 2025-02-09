using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Application.Interfaces.Auth;

namespace EventsWebApplication.Infrastructure.Data
{
    public class PasswordHasher : IPasswordHasher
    {

        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
        }

        public bool Verify(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
        }

    }
}

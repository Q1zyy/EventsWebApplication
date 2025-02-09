using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsWebApplication.Application.Interfaces.Auth
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        public bool Verify(string password, string hashedPassword);
    }
}

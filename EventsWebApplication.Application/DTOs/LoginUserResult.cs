using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Domain.Entities;

namespace EventsWebApplication.Application.DTOs
{
    public class LoginUserResult
    {
        public string AccessToken { get; set; } 
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; } 
        public User User { get; set; }
    }
}

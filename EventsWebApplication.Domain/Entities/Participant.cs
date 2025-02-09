using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsWebApplication.Domain.Entities
{
    public class Participant
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int EventId { get; set; }
        public DateTime RegistrationTime { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsWebApplication.Domain.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description  { get; set; }
        public DateTime EventDateTime { get; set; }
        public int ParticipantsMaxCount { get; set; }
        public List<string>? Images { get; set; }
        public string? Place { get; set; }
        public Category? Category { get; set; }
        public ICollection<Participant> Participants { get; set; } = [];
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsWebApplication.Application.DTOs
{
    public class PaginatedList<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }

    }
}

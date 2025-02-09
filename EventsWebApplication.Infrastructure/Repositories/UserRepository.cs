using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventsWebApplication.Application.Interfaces.Repositories;
using EventsWebApplication.Domain.Entities;
using EventsWebApplication.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApplication.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
            if (user == null)
            {
                throw new Exception("No such user");
            }
            return user;

        }

    }
}

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

        public override async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var user = await _context.Users.Include(u => u.Events).FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (user == null)
            {
                throw new Exception("No such user");
            }
            return user;
        }

        public async Task<IEnumerable<Event>> GetEventsByUserId(int id, CancellationToken cancellationToken)
        {
            var user = await _context.Users.Include(u => u.Events).ThenInclude(e => e.Category).FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (user == null)
            {
                throw new Exception("No such user");
            }
            return user.Events;
        }

        public async Task<User> GetUserByRefreshToken(string refresh, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken.Equals(refresh));
            if (user == null)
            {
                throw new Exception("No such user");
            }
            return user;
        }
    }
}

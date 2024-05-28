using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Application.Repositories
{
    public class UserRepo : GenericRepo<User>, IUser
    {
        private readonly AppDbContext _context;
        public UserRepo(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users
                        .Include(x => x.UserRoles)
                        .ThenInclude(x => x.Role)
                        .FirstOrDefaultAsync(x => x.Username.ToLower() == username.ToLower());
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users
                        .Include(x => x.UserRoles)
                        .ThenInclude(x => x.Role)
                        .FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
        }

        public async Task<User> GetUserWithoutRolesAsync(string username)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(x => x.Username.ToLower() == username.ToLower());

            if (user != null)
            {
                user.UserRoles = user.UserRoles.Where(ur => ur.Role == null).ToList();
            }

            return user;
        }

    }
}
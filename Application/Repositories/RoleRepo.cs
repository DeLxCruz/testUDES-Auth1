using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure;

namespace Application.Repositories
{
    public class RoleRepo : GenericRepo<Roles>, IRole
    {
        private readonly AppDbContext _context;
        public RoleRepo(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
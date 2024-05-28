using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using Domain.Entities;

namespace API.Services
{
    public interface IUserServices
    {
        Task<string> RegisterAsync(RegisterDTO model);
        Task<string> LoginAsync(LoginDTO model);
        string CreateToken(User model);
        Task<string> AddRoleToUserAsync(AddRoleToUserDTO model);
    }
}
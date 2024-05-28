using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.DTOs;
using API.Helpers;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;

        public UserServices(IUnitOfWork unitOfWork, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _config = config;
        }

        public string CreateToken(User model)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        public async Task<string> LoginAsync(LoginDTO model)
        {
            var user = await _unitOfWork.Users.GetByUsernameAsync(model.Username);

            if (user == null)
            {
                return "User not found";
            }

            bool isPassValid = BCrypt.Net.BCrypt.Verify(model.Password, user.Password);

            if (isPassValid)
            {
                string token = CreateToken(user);
                return token;
            }
            else
            {
                return "Invalid password";
            }
        }

        public async Task<string> RegisterAsync(RegisterDTO model)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            User user = new User
            {
                Username = model.Username,
                Email = model.Email,
                Password = passwordHash,
                Name = model.Name,
                LastName = model.LastName,
                TypeIdentification = model.TypeIdentification,
                DocumentNumber = model.DocumentNumber
            };

            var exisitingUser = _unitOfWork.Users
                    .Find(x => x.Username.ToLower() == model.Username.ToLower() || x.Email.ToLower() == model.Email.ToLower());

            if (!exisitingUser.Any())
            {
                var roleDefault = _unitOfWork.Roles
                    .Find(x => x.RoleName == Authorization.rol_default.ToString())
                    .FirstOrDefault();

                if (roleDefault != null)
                {
                    try
                    {
                        user.Roles.Add(roleDefault);
                        _unitOfWork.Users.Add(user);
                        await _unitOfWork.SaveAsync();
                        return $"User {model.Username} has been registered";
                    }
                    catch (Exception ex)
                    {
                        var msg = ex.Message;
                        return $"Failed to register user {model.Username} due to {msg}";
                    }
                }
                else
                {
                    return "Failed to register user, role not found";
                }
            }
            else
            {
                return "User already exists";
            }
        }

        public async Task<string> AddRoleToUserAsync(AddRoleToUserDTO model)
        {
            var user = await _unitOfWork.Users.GetByUsernameAsync(model.Username);

            if (user == null)
            {
                return $"User {model.Username} not found";
            }

            var role = _unitOfWork.Roles
                            .Find(r => r.RoleName.ToLower() == model.RoleName.ToLower())
                            .FirstOrDefault();

            if (role == null)
            {
                return $"Role {model.RoleName} not found";
            }

            if (user.UserRoles == null)
            {
                user.UserRoles = new List<UserRoles>();
            }

            var userRoleExists = user.UserRoles.Any(u => u.RoleId == role.Id);

            if (!userRoleExists)
            {
                user.UserRoles.Add(new UserRoles
                {
                    UserId = user.Id,
                    RoleId = role.Id
                });
                _unitOfWork.Users.Update(user);
                await _unitOfWork.SaveAsync();
                return $"Role {model.RoleName} has been added to user {model.Username}";
            }

            return $"User {model.Username} already has the role {model.RoleName}";
        }
    }
}
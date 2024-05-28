using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public AuthController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync(RegisterDTO model)
        {
            var result = await _userServices.RegisterAsync(model);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync(LoginDTO model)
        {
            var result = await _userServices.LoginAsync(model);
            return Ok(result);
        }

        [HttpPost("addRoleToUser")]
        public async Task<ActionResult> AddRoleToUserAsync(AddRoleToUserDTO model)
        {
            var result = await _userServices.AddRoleToUserAsync(model);
            return Ok(result);
        }
    }
}
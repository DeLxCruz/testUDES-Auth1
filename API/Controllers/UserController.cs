using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly IUserServices _userServices;

        public UserController(IUnitOfWork unitOfWork, IMapper mapper, AppDbContext context, IUserServices userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
            _userServices = userService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<UserDTO>>> Get()
        {
            var customer = await _unitOfWork.Users.GetAllAsync();
            return _mapper.Map<List<UserDTO>>(customer);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDTO>> Get(int id)
        {
            var customer = await _unitOfWork.Users.GetByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return _mapper.Map<UserDTO>(customer);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDTO>> Put(int id, [FromBody] UserDTO user)
        {
            if (user.Id == 0)
            {
                user.Id = id;
            }

            if (user.Id != id)
            {
                return BadRequest();
            }

            if (user == null)
            {
                return NotFound();
            }

            var customer = _mapper.Map<User>(user);
            _unitOfWork.Users.Update(customer);
            await _unitOfWork.SaveAsync();
            return user;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _unitOfWork.Users.GetByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            _unitOfWork.Users.Remove(customer);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        [HttpPost("addrole")]
        public async Task<IActionResult> AddRoleAsync(AddRoleToUserDTO model)
        {
            var result = await _userServices.AddRoleToUserAsync(model);
            return Ok(result);
        }

    }
}
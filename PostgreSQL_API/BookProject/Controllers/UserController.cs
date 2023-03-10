﻿using Microsoft.AspNetCore.Mvc;
using BookProject.Application.Interfaces;
using System.Threading.Tasks;
using BookProject.Application.Models;
using AutoMapper;
using BookProject.Application.Mapper;
using BookProject.Application.Validation;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IMapper mapper = BookProjectMapper.Mapper;
        private readonly IUserService _userService;
        private readonly UserValidator _userValidator;

        public UserController(IUserService userservice)
        {
            _userService = userservice;
            _userValidator = new UserValidator(_userService);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if(id <= 0)
            {
                return BadRequest("Invalid User ID");
            }
            var user = await _userService.GetByIdAsync(id);
            if(user == null)
            {
                return NotFound();
            }
            var userModel = mapper.Map<UserResponse>(user);
            return Ok(userModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var user = await _userService.GetAllAsync();
            return Ok(user);

        }

        [HttpPost]
        public async Task<IActionResult> Create(UserModel user)
        {
            if(user == null)
            {
                return BadRequest("User information is missing.");
            }

            var validationResult = await _userValidator.ValidateAsync(user);
            if(!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var newUser = await _userService.AddAsync(user);

            return Ok(newUser);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UserModel user)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Test Error");
            }

            var existingUser = await _userService.GetByIdAsync(id);
            if(existingUser == null)
            {
                return NotFound();
            }

            var updatedUserModel = new UserModel
            {
                Id = existingUser.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

            var updatedUser = await _userService.UpdateAsync(updatedUserModel);

            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingArticle = await _userService.GetByIdAsync(id);
            if(existingArticle == null)
            {
                return BadRequest("Error. Invalid ID!");
            }


            await _userService.DeleteAsync(id);

            return Ok("Deleted");
        }
    }
}

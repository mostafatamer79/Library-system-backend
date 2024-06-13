using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using library.Data;
using library.Models;
using AutoMapper;

namespace library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _userRepository.GetUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving users: {ex.Message}");
            }
        }

        [HttpGet("getactiveusers")]
        public async Task<IActionResult> GetActiveUsers()
        {
            try
            {
                var activeUsers = await _userRepository.GetActiveUsersAsync();
                return Ok(activeUsers);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving active users: {ex.Message}");
            }
        }

        [HttpGet("getinactiveusers")]
        public async Task<IActionResult> GetInactiveUsers()
        {
            try
            {
                var inactiveUsers = await _userRepository.GetInactiveUsersAsync();
                return Ok(inactiveUsers);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving inactive users: {ex.Message}");
            }
        }

        [HttpGet("getcountofusers")]
        public async Task<IActionResult> GetCountOfUsers()
        {
            try
            {
                var userCount = await _userRepository.GetUserCountAsync();
                return Ok(userCount);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving user count: {ex.Message}");
            }
        }

        [HttpGet("getcountofacceptedusers")]
        public async Task<IActionResult> GetCountOfAcceptedUsers()
        {
            try
            {
                var acceptedUserCount = await _userRepository.GetAcceptedUserCountAsync();
                return Ok(acceptedUserCount);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving accepted user count: {ex.Message}");
            }
        }

        [HttpGet("login/{email}/{password}")]
        public async Task<IActionResult> LoginAsync( string email,  string password)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(email);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);
                if (!isPasswordValid)
                {
                    return BadRequest("Invalid email or password.");
                }

                var userDto = _mapper.Map<User_Dto>(user);
                userDto.Id = user.Id;
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error logging in: {ex.Message}");
            }
        }

        [HttpPost("createaccount")]
        public async Task<IActionResult> CreateAccountAsync(User_Dto dto)
        {
            try
            {
                var existingUser = await _userRepository.GetUserByEmailAsync(dto.email);
                if (existingUser != null)
                {
                    return BadRequest("Email is already in use. Please choose a different email.");
                }

                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                var user = _mapper.Map<User>(dto);
                user.Password = hashedPassword;
                user.isAdmin = false;
                user.Acceptable = false;

                var createdUser = await _userRepository.CreateUserAsync(user);
                var createdUserDto = _mapper.Map<User_Dto>(createdUser);

                return Ok(createdUserDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating user: {ex.Message}");
            }
        }

        [HttpPut("activateuser/{id}")]
        public async Task<IActionResult> ActivateUser(int id)
        {
            try
            {
                var isActivated = await _userRepository.ActivateUserAsync(id);
                if (isActivated)
                {
                    return Ok($"User with ID {id} activated successfully.");
                }
                return NotFound($"User not found with ID: {id}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error activating user: {ex.Message}");
            }
        }

        [HttpPut("deactivateuser/{id}")]
        public async Task<IActionResult> DeactivateUser(int id)
        {
            try
            {
                var isDeactivated = await _userRepository.DeactivateUserAsync(id);
                if (isDeactivated)
                {
                    return Ok($"User with ID {id} deactivated successfully.");
                }
                return NotFound($"User not found with ID: {id}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deactivating user: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var isDeleted = await _userRepository.DeleteUserAsync(id);
                if (isDeleted)
                {
                    return Ok($"User with ID {id} deleted successfully.");
                }
                return NotFound($"User not found with ID: {id}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting user: {ex.Message}");
            }
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.api.Controllers;
using OrderManagement.Core.DTOs;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Interfaces;

namespace OrderManagement.API.Controllers
{
    public class UsersController : ApiBaseController
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepo;

        public UsersController(IGenericRepository<User> userRepository, ITokenService tokenService,IUserRepository userRepo)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _userRepo = userRepo;
        }
        #region register

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _userRepo.UsernameExistsAsync(userDto.Username))
            {
                return BadRequest("Username already exists.");
            }

            var user = new User
            {
                Username = userDto.Username,
                PasswordHash = _tokenService.HashPassword(userDto.PasswordHash),
                Role = Enum.TryParse<User.UserRole>(userDto.Role, true, out var role) ? role : Core.Entities.User.UserRole.Customer
            };

            await _userRepository.AddAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { userId = user.Id }, user);
        }
        #endregion

        #region login
        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userRepo.GetByUsernameAsync(loginDto.Username);
            if (user == null || !_tokenService.VerifyPassword(user.PasswordHash, loginDto.Password))
            {
                return Unauthorized();
            }
            if (string.IsNullOrEmpty(user.Username))
            {
                return BadRequest("User data is incomplete.");
            }

            var token = _tokenService.GenerateJwtToken(user);
            return Ok(new { Token = token });
        }
        #endregion

        #region GetUserById

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        } 
        #endregion



    }
}

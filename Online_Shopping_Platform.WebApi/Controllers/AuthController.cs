using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Online_Shopping_Platform.Business.Operations.User.Dtos;
using Online_Shopping_Platform.Business.Operations.User;
using Online_Shopping_Platform.WebApi.Models;
using Online_Shopping_Platform.WebApi.Jwt;

namespace Online_Shopping_Platform.WebApi.Controllers
{
    [Route("api/[controller]")]  // Define the route for the controller, dynamically using controller name
    [ApiController]  // Automatically handles model validation and error responses
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        // Constructor to inject IUserService for user-related operations
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        // Endpoint for user registration
        [HttpPost("register")]  // HTTP POST to register a new user
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            // Check if model state is valid (data validation)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  // Return validation errors
            }

            // Map request data to AddUserDto for adding user
            var addUserDto = new AddUserDto
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = request.Password,
                PhoneNumber = request.PhoneNumber
            };

            // Call service to add user and return appropriate response
            var result = await _userService.AddUser(addUserDto);

            if (result.IsSucceed)
                return Ok();  // Return success
            else
                return BadRequest(result.Message);  // Return failure message
        }

        // Endpoint for user login
        [HttpPost("login")]  // HTTP POST to log in a user
        public IActionResult Login(LoginRequest request)
        {
            // Check if model state is valid (data validation)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  // Return validation errors
            }

            // Call service to validate login credentials
            var result = _userService.LoginUser(new LoginUserDto { Email = request.Email, Password = request.Password });

            if (!result.IsSucceed)
                return BadRequest(result.Message);  // Return failure message

            var user = result.Data;  // Get user data after successful login

            // Get configuration values for JWT
            var configuration = HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            // Generate JWT token for authenticated user
            var token = JwtHelper.GenerateJwtToken(new JwtDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                SecretKey = configuration["Jwt:SecretKey"]!,
                Issuer = configuration["Jwt:Issuer"]!,
                Audience = configuration["Jwt:Audience"]!,
                ExpireMinutes = int.Parse(configuration["Jwt:ExpireMinutes"]!)
            });

            // Return success response with JWT token
            return Ok(new LoginResponse
            {
                Message = "Successful Login.",
                Token = token,  // Return the generated JWT token
            });
        }
    }
}

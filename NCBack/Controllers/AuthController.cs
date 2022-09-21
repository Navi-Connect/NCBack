using Microsoft.AspNetCore.Authorization;
using NCBack.Data;
using NCBack.Dtos.User;
using Microsoft.AspNetCore.Mvc;
using NCBack.Models;

namespace NCBack.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;

        public AuthController(IAuthRepository authRepo)
        {
            _authRepo = authRepo;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromForm] UserRegisterDto request)
        {
            var response = await _authRepo.Register(
                request.City,
                request.Region,
                request.PhoneNumber,
                request.Email,
                request.Username,
                request.FirstName,
                request.Lastname,
                request.SurName,
                request.DateOfBirth,
                request.File,
                request.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("verificationCode")]
        public async Task<ActionResult<User>> VerificationCode(UserCodeDto request)
        {
            var response = await _authRepo.VerificationCode(
                request.VerificationCode
            );
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(UserLoginDto request)
        {
            var response = await _authRepo.Login(request.Username, request.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("changePassword")]
        [Authorize]
        public async Task<ActionResult<User>> ChangePassword(UserChangePasswordDto request)
        {
            var response = await _authRepo.ChangePassword(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
        
        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto request)
        {
            var response = await _authRepo.ForgotPassword(request.Email);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
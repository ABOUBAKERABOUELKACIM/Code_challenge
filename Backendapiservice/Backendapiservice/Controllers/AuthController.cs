using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backendapiservice.Application.DTOs;
using Backendapiservice.Infrastructure.Data;
using Backendapiservice.Infrastructure.Services;

namespace Backendapiservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;

        public AuthController(AppDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check all user types (Admin, Doctor, Assistant)
            var admin = await _context.Admins.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (admin != null && BCrypt.Net.BCrypt.Verify(loginDto.Password, admin.PasswordHash))
            {
                var token = _jwtService.GenerateToken(admin);
                return Ok(new LoginResponseDto
                {
                    Token = token,
                    Role = admin.Role,
                    FullName = admin.FullName,
                    Email = admin.Email,
                    UserId = admin.Id
                });
            }

            var doctor = await _context.Doctors.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (doctor != null && BCrypt.Net.BCrypt.Verify(loginDto.Password, doctor.PasswordHash))
            {
                var token = _jwtService.GenerateToken(doctor);
                return Ok(new LoginResponseDto
                {
                    Token = token,
                    Role = doctor.Role,
                    FullName = doctor.FullName,
                    Email = doctor.Email,
                    UserId = doctor.Id
                });
            }

            var assistant = await _context.Assistants.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (assistant != null && BCrypt.Net.BCrypt.Verify(loginDto.Password, assistant.PasswordHash))
            {
                var token = _jwtService.GenerateToken(assistant);
                return Ok(new LoginResponseDto
                {
                    Token = token,
                    Role = assistant.Role,
                    FullName = assistant.FullName,
                    Email = assistant.Email,
                    UserId = assistant.Id
                });
            }

            return Unauthorized("Invalid email or password");
        }
    }
}
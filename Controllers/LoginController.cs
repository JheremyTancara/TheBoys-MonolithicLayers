using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Data;
using Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers
{
    [ApiController]
    [Route("auth")] 
    public class LoginController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public LoginController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> IniciarSesion([FromBody] LoginUserDTO userDto)
        {
            var usuario = await _context.Users.FirstOrDefaultAsync(u => u.Username == userDto.Username && u.Password == userDto.Password);

            if (usuario == null)
            {
                return BadRequest(new 
                {
                    success = false,
                    message = "Incorrect credentials",
                    result = ""
                });
            }

            var claims = new[] 
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserID", usuario.UserID.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds
            );

            return Ok(new 
            {
                success = true,
                message = "Success",
                result = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApi.Models;


namespace WebApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserContext _context;

        public AuthController(UserContext context)
        {
            _context = context;
        }

        [HttpPost, Route("login")]
        public IActionResult Login([FromBody]User user)
        {
            var loginItem = _context.Users.SingleOrDefault(s => s.userName == user.userName);

            if (user == null)
            {
                return BadRequest("Invalid client request");
            }

            if (loginItem != null && loginItem.password == user.password)
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var tokeOptions = new JwtSecurityToken(
                    issuer: "http://localhost:4200",
                    audience: "http://localhost:4200",
                    claims: new List<Claim>(),
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok
                (new
                {
                    Token = tokenString,
                    id = loginItem.Id,
                    username = loginItem.userName,
                    firstName = loginItem.firstName,
                    lastName = loginItem.lastName,
                    password = loginItem.password
                });
            }
            else
            {
                return BadRequest("Username or password is incorrect");
            }
        }

    }
}
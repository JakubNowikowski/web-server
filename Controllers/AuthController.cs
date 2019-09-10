using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApi.Models;
using WebApi.Contexts;


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
            _context.SaveChanges();

            //if (_context.LoginItems.Count() == 0)
            //{
            //    // Create a new LoginItem if collection is empty,
            //    // which means you can't delete all LoginItems.
            //    _context.LoginItems.Add(new UserItem
            //    {
            //        firstName = "Kaszub",
            //        lastName = "Morski",
            //        userName = "Kaszub",
            //        password = "Kaszub"
            //    });
            //    _context.SaveChanges();
            //}
        }

        [HttpPost, Route("login")]
        public IActionResult Login([FromBody]UserItem user)
        {
            var loginItem = _context.LoginItems.SingleOrDefault(s => s.userName == user.userName);

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
                    lastName = loginItem.lastName
                });
            }
            else
            {
                return BadRequest("Username or password is incorrect");
            }
        }

    }
}
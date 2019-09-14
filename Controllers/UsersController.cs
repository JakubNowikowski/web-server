using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Contexts;
using Microsoft.AspNetCore.Mvc.ModelBinding;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly UserContext _context;

        public UsersController(UserContext context)
        {
            _context = context;
            _context.SaveChanges();

            if (_context.Users.Count() == 0)
            {
                // Create a new UsersItem if collection is emdkdlslskddjfpty,
                // which means you can't delete all UsersItems.

                #region Creating fake useres

                _context.Users.Add(new User
                {
                    firstName = "user1",
                    lastName = "user1",
                    userName = "user1",
                    password = "aaaaaa"
                });
                _context.Users.Add(new User
                {
                    firstName = "user2",
                    lastName = "user2",
                    userName = "user2",
                    password = "aaaaaa"
                });
                _context.Users.Add(new User
                {
                    firstName = "user3",
                    lastName = "user3",
                    userName = "user3",
                    password = "aaaaaa"
                });
                _context.Users.Add(new User
                {
                    firstName = "user4",
                    lastName = "user4",
                    userName = "user4",
                    password = "aaaaaa"
                });

                #endregion

                _context.SaveChanges();
            }
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser([FromRoute] int id)
        {
            try
            {
                var user = await GetUserById(id);
                return Ok(user);
            }
            catch (InvalidOperationException)
            {
                return NotFound("User with specified ID was not found");
            }

        }

        private async Task<User> GetUserById(int id)
        {
            return await _context
              .Users
              .SingleAsync(u => u.Id == id);
        }

        // POST: api/Users
        //[HttpPost]
        //public async Task<ActionResult<UserItem>> PostUser(UserItem item)
        //{
        //    _context.Users.Add(item);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction(nameof(GetUser), new { id = item.Id }, item);
        //}

        [HttpPost]
        public async Task<ActionResult<User>> PostUser([BindRequired] [FromBody] User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoginItem(long id, User item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoginItem(long id)
        {
            var loginItem = await _context.Users.FindAsync(id);

            if (loginItem == null)
            {
                return NotFound();
            }

            _context.Users.Remove(loginItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

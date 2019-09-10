using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Contexts;

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

            if (_context.LoginItems.Count() == 0)
            {
                // Create a new LoginItem if collection is emdkdlslskddjfpty,
                // which means you can't delete all LoginItems.

                #region Creating fake useres

                _context.LoginItems.Add(new UserItem
                {
                    firstName = "user1",
                    lastName = "user1",
                    userName = "user1",
                    password = "aaaaaa"
                });
                _context.LoginItems.Add(new UserItem
                {
                    firstName = "user2",
                    lastName = "user2",
                    userName = "user2",
                    password = "aaaaaa"
                });
                _context.LoginItems.Add(new UserItem
                {
                    firstName = "user3",
                    lastName = "user3",
                    userName = "user3",
                    password = "aaaaaa"
                });
                _context.LoginItems.Add(new UserItem
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

        // GET: api/Login
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserItem>>> GetLoginItems()
        {
            return await _context.LoginItems.ToListAsync();
        }

        //// GET: api/Login
        //[HttpGet,Authorize]
        //public async Task<ActionResult<IEnumerable<LoginItem>>> GetLoginItems()
        //{
        //    return await _context.LoginItems.ToListAsync();
        //}

        // GET: api/Login/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserItem>> GetLoginItem(long id)
        {
            var loginItem = await _context.LoginItems.FindAsync(id);

            if (loginItem == null)
            {
                return NotFound();
            }

            return loginItem;
        }

        // POST: api/Login
        [HttpPost]
        public async Task<ActionResult<UserItem>> PostLoginItem(UserItem item)
        {
            _context.LoginItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLoginItem), new { id = item.Id }, item);
        }

        // PUT: api/Login/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoginItem(long id, UserItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Login/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoginItem(long id)
        {
            var loginItem = await _context.LoginItems.FindAsync(id);

            if (loginItem == null)
            {
                return NotFound();
            }

            _context.LoginItems.Remove(loginItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

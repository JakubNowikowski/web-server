using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Contexts;
using WebApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowController : Controller
    {
        private readonly FollowContext _context;

        public FollowController(FollowContext context)
        {
            _context = context;
            _context.SaveChanges();

            if (_context.FollowItems.Count() == 0)
            {
                // Create a new LoginItem if collection is emdkdlslskddjfpty,
                // which means you can't delete all LoginItems.
                _context.FollowItems.Add(new FollowItem
                {
                    follower = "user1",
                    following = "user2"
                });
                _context.FollowItems.Add(new FollowItem
                {
                    follower = "user2",
                    following = "user1"
                });
                _context.FollowItems.Add(new FollowItem
                {
                    follower = "user1",
                    following = "user3"
                });
                _context.FollowItems.Add(new FollowItem
                {
                    follower = "user1",
                    following = "user4"
                });
                _context.SaveChanges();
            }

        }

        // GET: api/Login
        [HttpGet("allItems")]
        public async Task<ActionResult<IEnumerable<FollowItem>>> GetAllItems()
        {
            return await _context.FollowItems.ToListAsync();
        }

        [HttpGet("followers")]
        public async Task<ActionResult<IEnumerable<FollowItem>>> GetFollowers(string userName)
        {
            return await _context.FollowItems
                .Where(f => f.following == userName)
                .ToListAsync();
        }

        [HttpGet("followings")]
        public async Task<ActionResult<IEnumerable<FollowItem>>> GetFollowings(string userName)
        {
            return await _context.FollowItems
                .Where(f => f.follower == userName)
                .ToListAsync();
        }

        // POST: api/Login
        [HttpPost]
        public async Task<ActionResult<FollowItem>> PostFollowItem(FollowItem item)
        {
            _context.FollowItems.Add(item);
            await _context.SaveChangesAsync();

            //return CreatedAtAction(nameof(GetLoginItem), new { id = item.Id }, item);
            return Ok();
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete]
        public async Task<IActionResult> Unfollow(string follower, string following)
        {
            //var loginItem = await _context.LoginItems.FindAsync(id);

            //if (loginItem == null)
            //{
            //    return NotFound();
            //}

            //_context.LoginItems.Remove(loginItem);
            //await _context.SaveChangesAsync();


            //var followItem = await _context.FollowItems.FindAsync(follower, following);

            var followItem = await _context.FollowItems.Where(f => f.follower == follower && f.following == following).FirstOrDefaultAsync();


            if (followItem == null)
            {
                return NotFound();
            }

            _context.FollowItems.Remove(followItem);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

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
    public class FollowingController : Controller
    {
        private readonly FollowContext _context;

        public FollowingController(FollowContext context)
        {
            _context = context;
        }

        //// GET: api/Login
        //[HttpGet("allItems")]
        //public async Task<ActionResult<IEnumerable<FollowItem>>> GetAllItems()
        //{
        //    return await _context.Followings.ToListAsync();
        //}

        //[HttpGet("followers")]
        //public async Task<ActionResult<IEnumerable<FollowItem>>> GetFollowers(string userName)
        //{
        //    return await _context.FollowItems
        //        .Where(f => f.followingId == userName)
        //        .ToListAsync();
        //}

        //// GET: api/1/following
        //[HttpGet("{id}")]
        //public async Task<ActionResult<IEnumerable<FollowItem>>> GetFollowings(string userName)
        //{
        //    return await _context.FollowItems
        //        .Where(f => f.followerId == userName)
        //        .ToListAsync();
        //}

        
            
            
        //// POST: api/Login
        //[HttpPost]
        //public async Task<ActionResult<FollowItem>> PostFollowItem(FollowItem item)
        //{
        //    _context.FollowItems.Add(item);
        //    await _context.SaveChangesAsync();

        //    //return CreatedAtAction(nameof(GetLoginItem), new { id = item.Id }, item);
        //    return Ok();
        //}

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }







        //// DELETE api/<controller>/5
        //[HttpDelete]
        //public async Task<IActionResult> Unfollow(int followerId, int followingId)
        //{
        //    //var loginItem = await _context.LoginItems.FindAsync(id);

        //    //if (loginItem == null)
        //    //{
        //    //    return NotFound();
        //    //}

        //    //_context.LoginItems.Remove(loginItem);
        //    //await _context.SaveChangesAsync();


        //    //var followItem = await _context.FollowItems.FindAsync(follower, following);

        //    var followItem = await _context.FollowItems.Where(f => f.followerId == followerId && f.followingId == followingId).FirstOrDefaultAsync();


        //    if (followItem == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.FollowItems.Remove(followItem);

        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}
    }
}

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
                    follower = "Kaszub",
                    following = "Bakusz"
                });
                _context.SaveChanges();
            }

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FollowItem>>> GetFollowings(string userName)
        {
            //TODO
            return await _context.FollowItems
                .Where(f => f.follower == userName)
                .ToListAsync();
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        //// POST api/<controller>
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

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
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

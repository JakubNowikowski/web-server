using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Contexts;
using WebApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : Controller
    {
        private readonly PostContext _context;

        public PostsController(PostContext context)
        {
            _context = context;
            _context.SaveChanges();

            if (_context.PostsItems.Count() == 0)
            {
                // Create a new LoginItem if collection is emdkdlslskddjfpty,
                // which means you can't delete all LoginItems.
                _context.PostsItems.Add(new PostItem
                {
                    userName="Kaszub",
                    content="Hello world"
                });
                _context.SaveChanges();
            }
        }

        // GET: api/Posts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostItem>>> GetPostItems()
        {
            return await _context.PostsItems.ToListAsync();
        }

        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PostItem>> GetPostItem(long id)
        {
            var postItem = await _context.PostsItems.FindAsync(id);

            if (postItem == null)
            {
                return NotFound();
            }

            return postItem;
        }

        // POST: api/Posts
        [HttpPost]
        public async Task<ActionResult<PostItem>> PostPostItem(PostItem item)
        {
            _context.PostsItems.Add(item);
            await _context.SaveChangesAsync();

            //return CreatedAtAction(nameof(GetPostItem), new { id = item.Id }, item);
            return Ok
               (new
               {
                   id = item.Id,
                   username= item.userName,
                   content= item.content
               });
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Posts/5
        [HttpDelete]
        public async Task<IActionResult> DeleteAllPosts()
        {
            _context.PostsItems.RemoveRange(_context.PostsItems);

            //.MyEntities.RemoveRange(dbContext.MyEntities);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

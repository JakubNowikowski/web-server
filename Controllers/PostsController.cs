using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        private readonly PostContext _postContext;
        private readonly FollowContext _followContext;

        public PostsController(PostContext postContext, FollowContext followContext)
        {
            _postContext = postContext;
            _followContext = followContext;

            #region Random posts

            //if (_postContext.PostsItems.Count() == 0)
            //{
            //    _postContext.PostsItems.Add(new PostItem
            //    {
            //        userName = "user1",
            //        content = "post: user1"
            //    });
            //    _postContext.PostsItems.Add(new PostItem
            //    {
            //        userName = "user2",
            //        content = "post: user2"
            //    });
            //    _postContext.PostsItems.Add(new PostItem
            //    {
            //        userName = "user3",
            //        content = "post: user3"
            //    });
            //    _postContext.PostsItems.Add(new PostItem
            //    {
            //        userName = "user4",
            //        content = "post: user4"
            //    });
            //    _postContext.SaveChanges();
            //}

            #endregion
        }

        // GET: api/Posts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostItem>>> GetPostsByFollowingUsers(string currentUser)
        {
            var postList = new List<PostItem>();
            var followingList = await GetFollowingsAsync(currentUser);

            foreach (var user in followingList)
            {
                postList.AddRange(await _postContext.PostsItems
                //.Where(p => p.userId == user.following)
                .OrderByDescending(p => p.Id).ToListAsync());
            }

            return postList;
        }

        private async Task<List<FollowItem>> GetFollowingsAsync(string currentUser)
        {
            return await _followContext.FollowItems
                .Where(f => f.follower == currentUser)
                .ToListAsync();
        }

        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PostItem>> GetPostItem(long id)
        {
            var postItem = await _postContext.PostsItems.FindAsync(id);

            if (postItem == null)
            {
                return NotFound();
            }

            return postItem;
        }

        // POST: api/Posts
        [HttpPost("{id}")]
        public async Task<ActionResult<PostItem>> PostPostItem(PostItem post)
        {
            _postContext.PostsItems.Add(post);
            await _postContext.SaveChangesAsync();

            return Ok
               (new
               {
                   id = post.Id,
                   userId = post.userId,
                   content = post.content
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
            _postContext.PostsItems.RemoveRange(_postContext.PostsItems);

            await _postContext.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

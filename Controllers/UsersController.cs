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
        private readonly UserContext _userContext;
        private readonly PostContext _postContext;
        private readonly FollowContext _followContext;

        public UsersController(UserContext userContext, PostContext postContext, FollowContext followContext)
        {
            _userContext = userContext;
            _postContext = postContext;
            _followContext = followContext;

            #region Creating fake users

            if (_userContext.Users.Count() == 0)
            {
                // Create a new UsersItem if collection is emdkdlslskddjfpty,
                // which means you can't delete all UsersItems.


                _userContext.Users.Add(new User
                {
                    firstName = "user1",
                    lastName = "user1",
                    userName = "user1",
                    password = "aaaaaa"
                });
                _userContext.Users.Add(new User
                {
                    firstName = "user2",
                    lastName = "user2",
                    userName = "user2",
                    password = "aaaaaa"
                });
                _userContext.Users.Add(new User
                {
                    firstName = "user3",
                    lastName = "user3",
                    userName = "user3",
                    password = "aaaaaa"
                });
                _userContext.Users.Add(new User
                {
                    firstName = "user4",
                    lastName = "user4",
                    userName = "user4",
                    password = "aaaaaa"
                });

                _userContext.SaveChanges();
            }

            #endregion

            #region Creating random posts

            if (_postContext.PostsItems.Count() == 0)
            {
                _postContext.PostsItems.Add(new PostItem
                {
                    userId = 1,
                    content = "post: user1"
                });
                _postContext.PostsItems.Add(new PostItem
                {
                    userId = 2,
                    content = "post: user2"
                });
                _postContext.PostsItems.Add(new PostItem
                {
                    userId = 3,
                    content = "post: user3"
                });
                _postContext.PostsItems.Add(new PostItem
                {
                    userId = 4,
                    content = "post: user4"
                });
                _postContext.SaveChanges();
            }

            #endregion

            #region Creating random follows

            if (_followContext.FollowItems.Count() == 0)
            {
                // Create a new LoginItem if collection is emdkdlslskddjfpty,
                // which means you can't delete all LoginItems.
                _followContext.FollowItems.Add(new FollowItem
                {
                    followerId = 1,
                    followingId = 2
                });
                _followContext.FollowItems.Add(new FollowItem
                {
                    followerId = 2,
                    followingId = 1
                });
                _followContext.FollowItems.Add(new FollowItem
                {
                    followerId = 1,
                    followingId = 3
                });
                _followContext.FollowItems.Add(new FollowItem
                {
                    followerId = 1,
                    followingId = 4
                });

                _followContext.SaveChanges();
            }

            #endregion
        }

        #region Users

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _userContext.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
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
            return await _userContext
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
        public async Task<IActionResult> PostUser([BindRequired] [FromBody] User user)
        {
            _userContext.Users.Add(user);
            await _userContext.SaveChangesAsync();

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

            _userContext.Entry(item).State = EntityState.Modified;
            await _userContext.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoginItem(long id)
        {
            var loginItem = await _userContext.Users.FindAsync(id);

            if (loginItem == null)
            {
                return NotFound();
            }

            _userContext.Users.Remove(loginItem);
            await _userContext.SaveChangesAsync();

            return NoContent();
        }

        #endregion

        #region Posts

        // GET: api/Posts
        [HttpGet("{id}/posts/{postId}")]
        public async Task<IActionResult> GetPost([FromRoute] int id, [FromRoute] int postId)
        {
            User user;
            try
            {
                user = await GetUserById(id);
            }
            catch (InvalidOperationException)
            {
                return NotFound("Specified user could not be found");
            }

            PostItem post;
            try
            {
                post = await _postContext.PostsItems.SingleAsync(p => p.userId == id && p.Id == postId);
            }
            catch (Exception e) when (e is InvalidOperationException || e is ArgumentNullException)
            {
                return NotFound("Specified post could not be found");
            }

            return Ok();
        }

        // GET: api/Posts
        [HttpGet("{id}/posts")]
        public async Task<ActionResult<IEnumerable<PostItem>>> GetPosts([FromRoute] int id)
        {
            var postList = new List<PostItem>();
            var followingList = await GetFollowingsAsync(id);

            postList = await _postContext.PostsItems.Where(p => p.userId == id).ToListAsync();

            foreach (var follow in followingList)
            {
                postList.AddRange(await _postContext.PostsItems
                .Where(p => p.userId == follow.followingId)
                .ToListAsync());
            }

            return postList.OrderByDescending(p => p.Id).ToList();
        }

        private async Task<List<FollowItem>> GetFollowingsAsync(int userId)
        {
            return await _followContext.FollowItems
                .Where(f => f.followerId == userId)
                .ToListAsync();
        }

        // POST: api/Posts
        [HttpPost("{id}/posts")]
        public async Task<IActionResult> PostPostItem([FromRoute] int id, [BindRequired] [FromBody] PostItem post)
        {
            User user;
            try
            {
                user = await GetUserById(id);
            }
            catch (InvalidOperationException)
            {
                return NotFound("Specified user could not be found");
            }

            post.userId = id;
            _postContext.PostsItems.Add(post);
            await _postContext.SaveChangesAsync();

            return Ok
               (new
               {
                   username = post.userId,
                   content = post.content
               });
            //return CreatedAtAction("GetPost", new { id = post.Id }, post);
        }


        #endregion
    }
}

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
                    userName = "",
                    content = "post: user1"
                });
                _postContext.PostsItems.Add(new PostItem
                {
                    userId = 2,
                    userName = "",
                    content = "post: user2"
                });
                _postContext.PostsItems.Add(new PostItem
                {
                    userId = 3,
                    userName = "",
                    content = "post: user3"
                });
                _postContext.PostsItems.Add(new PostItem
                {
                    userId = 4,
                    userName = "",
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

        //// GET: api/Users
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

        public async Task<User> GetUserById(int id)
        {
            return await _userContext
              .Users
              .SingleOrDefaultAsync(u => u.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([BindRequired] [FromBody] User user)
        {
            if (user == null)
                return BadRequest();
            if (await _userContext.Users.Where(u => u.userName == user.userName).FirstOrDefaultAsync() != null)
                return BadRequest("This username already exists");

            user = await AddUser(user);

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        public async Task<User> AddUser(User newUser)
        {
            _userContext.Users.Add(newUser);
            await _userContext.SaveChangesAsync();
            return newUser;
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoginItem(long id, [BindRequired] [FromBody] User user)
        {
            if (user == null)
                return BadRequest();
            if (id != user.Id)
                return BadRequest();

            user = await EditUser(user);

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        public async Task<User> EditUser(User user)
        {
            _userContext.Entry(user).State = EntityState.Modified;
            await _userContext.SaveChangesAsync();
            return user;
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoginItem(long id)
        {
            var loginItem = await _userContext.Users.FindAsync(id);

            if (loginItem == null)
                return NotFound();

            await DeleteUser(loginItem);

            return NoContent();
        }

        public async Task DeleteUser(User loginItem)
        {
            _userContext.Users.Remove(loginItem);
            await _userContext.SaveChangesAsync();
        }

        #endregion

        #region Posts

        // GET: api/users/1/followedPosts
        [HttpGet("{id}/followedPosts")]
        public async Task<ActionResult<IEnumerable<PostItem>>> GetFollowedPosts([FromRoute] int id)
        {
            List<PostItem> userPosts;
            List<PostItem> followingPosts;

            var followingList = await GetFollowingById(id);

            userPosts = GetPostsById(id);
            followingPosts = GetFollowingsPosts(followingList);

            userPosts.AddRange(followingPosts);

            return userPosts.OrderByDescending(p => p.Id).ToList();
        }

        public async Task<List<FollowItem>> GetFollowingById(int userId)
        {
            return await _followContext.FollowItems
                .Where(f => f.followerId == userId)
                .ToListAsync();
        }

        public List<PostItem> GetPostsById(int id)
        {
            return _postContext.PostsItems.AsEnumerable()
                            .Where(p => p.userId == id)
                            .Select(AddUserNameToPostItem(id))
                            .ToList();
        }

        private Func<PostItem, PostItem> AddUserNameToPostItem(long id)
        {
            return p => { p.userName = GetUserName(id); return p; };
        }

        public string GetUserName(long id)
        {
            return _userContext.Users.SingleOrDefault(u => u.Id == id).userName;
        }

        public List<PostItem> GetFollowingsPosts(List<FollowItem> followingList)
        {
            var postList = new List<PostItem>();

            foreach (var follow in followingList)
            {
                postList.AddRange(_postContext.PostsItems.AsEnumerable()
                .Where(p => p.userId == follow.followingId)
                .Select(AddUserNameToPostItem(follow.followingId))
                .ToList());
            }

            return postList;
        }

        // GET: api/users/1/posts
        [HttpGet("{id}/posts")]
        public async Task<ActionResult<IEnumerable<PostItem>>> GetPosts([FromRoute] int id)
        {
            return await _postContext.PostsItems.Where(u => u.userId == id).OrderByDescending(p => p.Id).ToListAsync();
        }

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

            return Ok(post);
        }

        private async Task<List<FollowItem>> GetFollowersIdAsync(int userId)
        {
            return await _followContext.FollowItems
                .Where(f => f.followingId == userId)
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
            await AddPost(post);

            return Ok
               (new
               {
                   content = post.content
               });
        }

        public async Task AddPost(PostItem post)
        {
            _postContext.PostsItems.Add(post);
            await _postContext.SaveChangesAsync();
        }

        // DELETE: api/users/1/posts/5
        [HttpDelete("{userId}/posts/{postId}")]
        public async Task<IActionResult> DeletePostItem([FromRoute] int userId, [FromRoute] int postId)
        {
            var post = await _postContext.PostsItems.Where(p => p.userId == userId && p.Id == postId).FirstOrDefaultAsync();

            if (post == null)
                return NotFound();

            _postContext.PostsItems.Remove(post);

            await _postContext.SaveChangesAsync();

            return Ok();
        }

        #endregion

        #region Follows

        // GET: api/1/following
        [HttpGet("{id}/following")]
        public async Task<ActionResult<IEnumerable<User>>> GetFollowings([FromRoute] int id)
        {
            var followingIds = await GetFollowingById(id);

            var usersList = new List<User>();

            foreach (var follow in followingIds)
                usersList.Add(await _userContext.Users.SingleAsync(u => u.Id == follow.followingId));

            return usersList;
        }

        // GET: api/users/1/followers
        [HttpGet("{id}/followers")]
        public async Task<ActionResult<IEnumerable<User>>> GetFollowers([FromRoute] int id)
        {
            var followersIds = await GetFollowersIdAsync(id);

            var usersList = new List<User>();

            foreach (var follow in followersIds)
                usersList.Add(await _userContext.Users.SingleAsync(u => u.Id == follow.followerId));

            return usersList;
        }

        // POST: api/users/1/follwing/5
        [HttpPost("{id}/following/{followingId}")]
        public async Task<ActionResult<FollowItem>> Follow([FromRoute] int id, [FromRoute] int followingId)
        {
            if (await _followContext.FollowItems.Where(f => f.followerId == id && f.followingId == followingId).FirstOrDefaultAsync() != null)
                return BadRequest();

            await AddFollowItem(id, followingId);

            return Ok();
        }

        private async Task AddFollowItem(int id, int followingId)
        {
            _followContext.FollowItems.Add(new FollowItem() { followerId = id, followingId = followingId });
            await _followContext.SaveChangesAsync();
        }

        // DELETE: api/users/1/follwing/5
        [HttpDelete("{id}/following/{followingId}")]
        public async Task<ActionResult<FollowItem>> Unfollow([FromRoute] int id, [FromRoute] int followingId)
        {
            var followItem = await _followContext.FollowItems.Where(f => f.followerId == id && f.followingId == followingId).FirstOrDefaultAsync();

            if (followItem == null)
                return NotFound();

            await DeleteFollowItem(followItem);

            return Ok();
        }

        private async Task DeleteFollowItem(FollowItem followItem)
        {
            _followContext.FollowItems.Remove(followItem);
            await _followContext.SaveChangesAsync();
        }

        // GET: api/users/1/explore
        [HttpGet("{id}/explore")]
        public async Task<ActionResult<IEnumerable<User>>> Explore([FromRoute] int id)
        {
            var usersToFollow = new List<User>();
            var followingIds = await GetFollowingById(id);

            List<long> usersToFollowIds = await ExtractFollowingsIdsByUserId(id, followingIds);

            foreach (var userToFollowId in usersToFollowIds)
                usersToFollow.Add(await _userContext.Users.SingleAsync(u => u.Id == userToFollowId));

            return usersToFollow;
        }

        public async Task<List<long>> ExtractFollowingsIdsByUserId(int id, List<FollowItem> followings)
        {
            return await _userContext.Users
                            .Where(u => u.Id != id)
                            .Select(u => u.Id)
                            .Except(followings.Select(f => f.followingId))
                            .ToListAsync();
        }

        #endregion
    }
}

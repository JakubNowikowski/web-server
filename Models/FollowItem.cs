using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class FollowItem
    {
        public long Id { get; set; }
        public int followerId { get; set; }
        public int followingId { get; set; }
    }
}

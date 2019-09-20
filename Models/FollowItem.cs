using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class FollowItem
    {
        public long Id { get; set; }
        public long followerId { get; set; }
        public long followingId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class FollowItem
    {
        public long Id { get; set; }
        public string follower { get; set; }
        public string following { get; set; }
    }
}

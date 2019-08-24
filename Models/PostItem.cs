using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class PostItem
    {
        public long Id { get; set; }
        public string userName { get; set; }
        public string content { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Contexts
{
    public class FollowContext:DbContext
    {
        public FollowContext(DbContextOptions<FollowContext> options) : base(options)
        {

        }

        public DbSet<FollowItem> FollowItems { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Services
{
    public class EntityFrameworkItemSource : DbContext, IItemSource
    {
        public void EditItem(IContext context, IValidatableObject item)
        {
            Entry(item).State = EntityState.Modified;
        }
    }
}

using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi
{
    public interface IContext
    {
        DbSet<User> Users { get; set; }
        void SaveChanges();
        Task SaveChangesAsync();
        //EntityState Entry(IValidatableObject item);
    }
}

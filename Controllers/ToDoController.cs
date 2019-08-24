 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Contexts;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : Controller
    {
        private readonly ToDoContext _context;

        public TodoController(ToDoContext context)
        {
            _context = context;
            _context.SaveChanges();

            if (_context.TodoItems.Count() == 0)
            {
                // Create a new TodoItem if collection is empty,
                // which means you can't delete all TodoItems.
                #region Adding random items

                _context.TodoItems.Add(new TodoItem
                {
                    productName = "Leaf Rake",
                    productCode = "GDN-0011",
                    releaseDate = "March 19, 2016",
                    description = "Leaf rake with 48-inch wooden handle.",
                    price = 19.95f,
                    starRating = 3.5f,
                    imageUrl = "https://openclipart.org/image/300px/svg_to_png/26215/Anonymous_Leaf_Rake.png"
                });

                _context.TodoItems.Add(new TodoItem
                {
                    productName = "Leaf Rake",
                    productCode = "GDN-0011",
                    releaseDate = "March 19, 2016",
                    description = "Leaf rake with 48-inch wooden handle.",
                    price = 19.95f,
                    starRating = 3f,
                    imageUrl = "https://openclipart.org/image/300px/svg_to_png/26215/Anonymous_Leaf_Rake.png"
                });
                _context.TodoItems.Add(new TodoItem
                {
                    productName = "Leaf Rake",
                    productCode = "GDN-0011",
                    releaseDate = "March 19, 2016",
                    description = "Leaf rake with 48-inch wooden handle.",
                    price = 19.95f,
                    starRating = 3.5f,
                    imageUrl = "https://openclipart.org/image/300px/svg_to_png/26215/Anonymous_Leaf_Rake.png"
                });
                _context.TodoItems.Add(new TodoItem
                {
                    productName = "Leaf Rake",
                    productCode = "GDN-0011",
                    releaseDate = "March 19, 2016",
                    description = "Leaf rake with 48-inch wooden handle.",
                    price = 19.95f,
                    starRating = 4f,
                    imageUrl = "https://openclipart.org/image/300px/svg_to_png/26215/Anonymous_Leaf_Rake.png"
                });
                _context.TodoItems.Add(new TodoItem
                {
                    productName = "Leaf Rake",
                    productCode = "GDN-0011",
                    releaseDate = "March 19, 2016",
                    description = "Leaf rake with 48-inch wooden handle.",
                    price = 19.95f,
                    starRating = 4.5f,
                    imageUrl = "https://openclipart.org/image/300px/svg_to_png/26215/Anonymous_Leaf_Rake.png"
                });
                _context.TodoItems.Add(new TodoItem
                {
                    productName = "Leaf Rake",
                    productCode = "GDN-0011",
                    releaseDate = "March 19, 2016",
                    description = "Leaf rake with 48-inch wooden handle.",
                    price = 19.95f,
                    starRating = 5f,
                    imageUrl = "https://openclipart.org/image/300px/svg_to_png/26215/Anonymous_Leaf_Rake.png"
                });

                #endregion
                _context.SaveChanges();
            }
        }

        // GET: api/Todo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return await _context.TodoItems.ToListAsync();
        }

        // GET: api/Todo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // POST: api/Todo
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem item)
        {
            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id }, item);
        }

        // PUT: api/Todo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Todo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

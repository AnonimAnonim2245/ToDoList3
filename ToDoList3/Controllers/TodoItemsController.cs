using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList3.Models;
using ToDoList3.Database;
using ToDoList3.Models.Dto;


namespace ToDoList3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetTodoItems()
        {
          if (_context.TodoItems == null)
          {
              return NotFound();
          }
          // Metoda 1
            //List<TodoItem> todoItems = await _context.TodoItems.ToListAsync();
            //List<TodoItemDto> result = new List<TodoItemDto>();

            //foreach(TodoItem item in todoItems)
            // {
            // result.Add(ItemToDTO(item));
            // }
            // return result;
            // sau
            // Metoda 2
            return await _context.TodoItems.Select(x => ItemToDTO(x)).ToListAsync();

        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDto>> GetTodoItem(int id)
        {
          if (_context.TodoItems == null)
          {
              return NotFound();
          }
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return Ok(ItemToDTO(todoItem));
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(int id, TodoItemDto todoItemDto)
        {
            if (id != todoItemDto.Id)
            {
                return BadRequest();
            }

            var todoItemSet = await _context.TodoItems.FindAsync(id);
            _context.Entry(todoItemSet).State = EntityState.Modified;

            todoItemSet.Name = todoItemDto.Name;
            todoItemSet.IsComplete = todoItemDto.IsComplete;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        
        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItemDto>> PostTodoItem(TodoItemDto todoItemDto)
        {
          if (_context.TodoItems == null)
          {
              return Problem("Entity set 'TodoContext.TodoItems'  is null.");
          }

            TodoItem todoItem = new TodoItem()
            {
                Name = todoItemDto.Name,
                IsComplete = todoItemDto.IsComplete,
            };
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            if (_context.TodoItems == null)
            {
                return NotFound();
            }
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        /*[HttpPatch("{id}")]
        public async Task<ActionResult> UpdateRecipe(int id, JsonPatchDocument<TodoItem> recipeUpdates)
        {
            var recipe = await _context.GetType(id);
            if (recipe == null)
            {
                return NoContent();
            }
            recipeUpdates.ApplyTo(recipe);
            await _context.Update(recipe);
            return NoContent();
        }*/
        private bool TodoItemExists(int id)
        {
            return (_context.TodoItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private static TodoItemDto ItemToDTO(TodoItem todoItem)
        {
            return new TodoItemDto
            {
                Id = todoItem.Id,
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete,
            };
        }
    }
}

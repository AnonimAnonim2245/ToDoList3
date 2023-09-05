using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList3.Database;
using ToDoList3.Models;
using ToDoList3.Models.Dto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToDoList3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly TodoContext _context;

        public UserController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/<UserController>
        [HttpGet("{id}/GetUserItems")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetUserItems(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(x => x.TodoItems)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.TodoItems);
        }

        // GET: api/<UserController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            return await _context.Users
                .Include(x => x.TodoItems)
               .ToListAsync();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> Get(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return ItemToDTO(user);
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] UserDto user)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'TodoContext.TodoItems'  is null.");
            }

            var localUser = new User
            {
                Name = user.Name,
                SecondName = user.SecondName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                ProfilePicture = user.ProfilePicture,
                BirthDate = user.BirthDate,
            };

            _context.Users.Add(localUser);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private static UserDto ItemToDTO(User user) =>
         new()
         {
             Id = user.Id,
             Name = user.Name,
             SecondName = user.SecondName,
             Email = user.Email,
             PhoneNumber = user.PhoneNumber,
             Address = user.Address,
             ProfilePicture = user.ProfilePicture,
             BirthDate = user.BirthDate,
         };
    }
}

using Microsoft.EntityFrameworkCore;
using ToDoList3.Models;

namespace ToDoList3.Database
{
    public class TodoContext : DbContext
    {
      
        public TodoContext(DbContextOptions<TodoContext> options) 
            : base(options)
        {
        }
        public DbSet<TodoItem> TodoItems { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;


    }

}

namespace ToDoList3.Models.Dto
{
    public class TodoItemDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public bool IsComplete { get; set; }
    }
}

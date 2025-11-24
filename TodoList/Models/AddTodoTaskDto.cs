namespace TodoList.Shared.Models
{
    public class AddTodoTaskDto
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public required int Priority { get; set; }
        public required bool Completion { get; set; } = false;
        public DateTime? Deadline { get; set; }
    }
}

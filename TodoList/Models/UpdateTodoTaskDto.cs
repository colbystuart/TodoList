namespace TodoList.Shared.Models
{
    public class UpdateTodoTaskDto
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public required int Priority { get; set; }
        public required bool Completion { get; set; }
        public DateTime? Deadline { get; set; }
    }
}

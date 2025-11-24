namespace TodoList.Shared.Models.Entities
{
    public class TodoTask
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public required int Priority { get; set; }
        public required bool Completion { get; set; }
        public DateTime? Deadline { get; set; }
    }
}

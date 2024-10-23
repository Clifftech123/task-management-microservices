namespace TaskService.API.Domain.Entities
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public int Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        // Foreign key for Project
        public int ProjectId { get; set; }
        // Foreign key for ApplicationUser (User ID)
        public string UserId { get; set; }

        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}

namespace ProjectService.API.Domain.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        // Foreign key for ApplicationUser (User ID)
        public string UserId { get; set; }

        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}

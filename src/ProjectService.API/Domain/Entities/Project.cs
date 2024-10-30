namespace ProjectService.API.Domain.Entities
{

    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        // Foreign key for ApplicationUser (User ID)
        public string UserId { get; set; }
        public ICollection<Tags> Tags { get; set; } = new List<Tags>();
    }

}


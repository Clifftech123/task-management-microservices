namespace ProjectService.API.Domain.Entities
{
    public class Tags
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // Foreign key for ApplicationUser (User ID)
        public string UserId { get; set; }
        public ICollection<Project> Project { get; set; }
    }
}

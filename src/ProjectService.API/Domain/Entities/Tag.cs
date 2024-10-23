namespace ProjectService.API.Domain.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Project> Project { get; set; }
    }
}

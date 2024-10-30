namespace ProjectService.API.Domain.Contracts
{
    public class ProjectDtos
    {
        public class CreateProjectRequest
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public DateTime EndDate { get; set; }
            public DateTime StartDate { get; set; }
            public string UserId { get; set; }
            public List<string> Tags { get; set; }
        }

        public class UpdateProjectRequest
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public DateTime EndDate { get; set; }
            public DateTime StartDate { get; set; }
            public string UserId { get; set; }
            public List<string> Tags { get; set; }
        }

        public class ProjectResponse
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public DateTime EndDate { get; set; }
            public DateTime StartDate { get; set; }
            public DateTimeOffset CreatedAt { get; set; }
            public DateTimeOffset UpdatedAt { get; set; }
            public string UserId { get; set; }
            public List<string> Tags { get; set; }
        }


    }
}

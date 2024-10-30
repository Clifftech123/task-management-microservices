namespace TaskService.API.Domain.Contracts
{
    public class CreateTaskRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public int Status { get; set; }
        public int ProjectId { get; set; }

        public string UserId { get; set; }

    }
}

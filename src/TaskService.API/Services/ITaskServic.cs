using TaskService.API.Domain.Contracts;
using TaskService.API.Domain.Entities;

namespace TaskService.API.Services
{
    public interface ITaskService
    {
        Task<TaskEntities> GetTaskAsync(int taskId);
        Task<TaskResponseRequest> CreateTaskAsync(CreateTaskRequest taskRequest);
        Task<TaskResponseRequest> UpdateTaskAsync(TaskResponseRequest taskRequest);
        Task<TaskResponseRequest> DeleteTaskAsync(int taskId);
        Task<IEnumerable<TaskEntities>> GetTasksAsync();
    }
}

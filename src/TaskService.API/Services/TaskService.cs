using Microsoft.EntityFrameworkCore;
using TaskService.API.Domain.Contracts;
using TaskService.API.Domain.Entities;
using TaskService.API.Infrastructure.Context;

namespace TaskService.API.Services
{
    public class TaskService : ITaskServices
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<TaskService> _logger;
        public readonly TaskDbContext Context;

        public TaskService(ICurrentUserService currentUserService, ILogger<TaskService> logger, TaskDbContext context)
        {
            _currentUserService = currentUserService;
            _logger = logger;
            Context = context;
        }

        public async Task<TaskResponseRequest> CreateTaskAsync(CreateTaskRequest taskRequest)
        {
            var taskResponse = new TaskResponseRequest
            {
                Id = taskRequest.Id,
                Title = taskRequest.Title,
                Description = taskRequest.Description,
                DueDate = taskRequest.DueDate,
                IsCompleted = taskRequest.IsCompleted,
                Status = taskRequest.Status,
                CreatedAt = DateTimeOffset.Now,
                UpdatedAt = DateTimeOffset.Now,
                ProjectId = taskRequest.ProjectId,
                UserId = _currentUserService.GetUserId()
            };

            Context.Tasks.Add(new TaskEntities
            {
                Id = taskResponse.Id,
                Title = taskResponse.Title,
                Description = taskResponse.Description,
                DueDate = taskResponse.DueDate,
                IsCompleted = taskResponse.IsCompleted,
                Status = taskResponse.Status,
                CreatedAt = taskResponse.CreatedAt,
                UpdatedAt = taskResponse.UpdatedAt,
                ProjectId = taskResponse.ProjectId,
                UserId = taskResponse.UserId
            });

            await Context.SaveChangesAsync();

            return taskResponse;
        }

        public async Task<IEnumerable<TaskEntities>> GetTasksAsync()
        {
            var taskList = await Context.Tasks.ToArrayAsync();
            if (taskList == null || taskList.Length == 0)
            {
                return Enumerable.Empty<TaskEntities>();
            }
            return taskList;
        }

        public async Task<TaskEntities> GetTaskAsync(int taskId)
        {
            var task = await Context.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);
            if (task == null)
            {
                return null;
            }
            return task;
        }

        public async Task<TaskResponseRequest> UpdateTaskAsync(TaskResponseRequest taskRequest)
        {
            var task = await Context.Tasks.FirstOrDefaultAsync(x => x.Id == taskRequest.Id);
            if (task == null)
            {
                throw new Exception("Task not found");
            }
            task.Title = taskRequest.Title;
            task.Description = taskRequest.Description;
            task.DueDate = taskRequest.DueDate;
            task.IsCompleted = taskRequest.IsCompleted;
            task.Status = taskRequest.Status;
            task.UpdatedAt = DateTimeOffset.Now;
            task.ProjectId = taskRequest.ProjectId;
            task.UserId = taskRequest.UserId;
            await Context.SaveChangesAsync();
            return taskRequest;
        }

        public async Task<TaskResponseRequest> DeleteTaskAsync(int taskId)
        {
            var task = await Context.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);
            if (task == null)
            {
                throw new Exception("Task not found");
            }
            Context.Tasks.Remove(task);
            await Context.SaveChangesAsync();
            return new TaskResponseRequest
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                IsCompleted = task.IsCompleted,
                Status = task.Status,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt,
                ProjectId = task.ProjectId,
                UserId = task.UserId
            };
        }
    }
}

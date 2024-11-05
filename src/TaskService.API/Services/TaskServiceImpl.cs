using Microsoft.EntityFrameworkCore;
using TaskService.API.Domain.Contracts;
using TaskService.API.Domain.Entities;
using TaskService.API.Infrastructure.Context;

namespace TaskService.API.Services
{
    public class TaskServiceImpl : ITaskService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<TaskServiceImpl> _logger;
        public readonly TaskDbContext Context;

        public TaskServiceImpl(ICurrentUserService currentUserService, ILogger<TaskServiceImpl> logger, TaskDbContext context)
        {
            _currentUserService = currentUserService;
            _logger = logger;
            Context = context;
        }

        public async Task<TaskResponseRequest> CreateTaskAsync(CreateTaskRequest taskRequest)
        {
            var userId = _currentUserService.GetUserId();
            var now = DateTimeOffset.Now;

            var taskEntity = new TaskEntities
            {
                Id = taskRequest.Id,
                Title = taskRequest.Title,
                Description = taskRequest.Description,
                DueDate = taskRequest.DueDate,
                IsCompleted = taskRequest.IsCompleted,
                Status = taskRequest.Status,
                CreatedAt = now,
                UpdatedAt = now,
                ProjectId = taskRequest.ProjectId,
                UserId = userId
            };

            Context.Tasks.Add(taskEntity);
            await Context.SaveChangesAsync();

            return new TaskResponseRequest
            {
                Id = taskEntity.Id,
                Title = taskEntity.Title,
                Description = taskEntity.Description,
                DueDate = taskEntity.DueDate,
                IsCompleted = taskEntity.IsCompleted,
                Status = taskEntity.Status,
                CreatedAt = taskEntity.CreatedAt,
                UpdatedAt = taskEntity.UpdatedAt,
                ProjectId = taskEntity.ProjectId,
                UserId = taskEntity.UserId
            };
        }

        public async Task<IEnumerable<TaskEntities>> GetTasksAsync()
        {
            _logger.LogInformation("Getting all tasks");
            var taskList = await Context.Tasks.ToListAsync();
            return taskList.Any() ? taskList : Enumerable.Empty<TaskEntities>();
        }

        public async Task<TaskEntities> GetTaskAsync(int taskId)
        {
            _logger.LogInformation("Getting task by id");
            var task = await Context.Tasks.FindAsync(taskId);
            if (task == null)
            {
                _logger.LogError("Task not found");
                throw new Exception("Task not found");
            }
            return task;
        }

        public async Task<TaskResponseRequest> UpdateTaskAsync(TaskResponseRequest taskRequest)
        {
            _logger.LogInformation("Updating task");
            var task = await Context.Tasks.FindAsync(taskRequest.Id);
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

        public async Task<bool> DeleteTaskAsync(int taskId)
        {
            _logger.LogInformation("Deleting task");
            var task = await Context.Tasks.FindAsync(taskId);
            if (task == null)
            {
                _logger.LogError("Task not found");
                return false;
            }

            Context.Tasks.Remove(task);
            await Context.SaveChangesAsync();

            return true;
        }

    }
}

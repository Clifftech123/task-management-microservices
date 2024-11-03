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
            _logger.LogInformation("Getting all tasks");
            var taskList = await Context.Tasks.ToArrayAsync();
            if (taskList == null || taskList.Length == 0)
            {
                throw new Exception("No tasks found");

            }
            return taskList;
        }

        public async Task<TaskEntities> GetTaskAsync(int taskId)
        {
            _logger.LogInformation("Getting task by id");
            var task = await Context.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);
            if (task == null)
            {
                return null;
            }
            return task;
        }

        public async Task<TaskResponseRequest> UpdateTaskAsync(TaskResponseRequest taskRequest)
        {
            _logger.LogInformation("Updating task");
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
            _logger.LogInformation("Deleting task");
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

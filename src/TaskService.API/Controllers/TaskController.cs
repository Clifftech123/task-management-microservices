using Microsoft.AspNetCore.Mvc;
using TaskService.API.Services;

namespace TaskService.API.Controllers
{
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public async Task<IActionResult> GetTasksAsync()
        {
            var tasks = await _taskService.GetTasksAsync();
            if (tasks == null || !tasks.Any())
            {
                return Ok(new { message = "No tasks found", tasks = new List<Task>() });
            }
            return Ok(tasks);
        }


    }
}

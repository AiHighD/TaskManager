using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasksManager.Services;
using TasksManager.ViewModels;

namespace TasksManager.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TasksApiController : ControllerBase
    {
        private readonly ITasksService _tasksService;

        public TasksApiController(ITasksService tasksService)
        {
            _tasksService = tasksService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] TaskRequest request)
        {
           if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _tasksService.Create(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _tasksService.GetById(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _tasksService.GetAll();
            return Ok(result);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetAllFilter(string? sortOrder, string? currentFilter, string? searchString, int? pageNumber, int pageSize = 3)
        {
            var result = await _tasksService.GetAllFilter(sortOrder!, currentFilter!, searchString!, pageNumber, pageSize);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _tasksService.Delete(id);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] TaskViewModel request)
        {
            var result = await _tasksService.Update(request);
            return Ok(result);
        }

    }
}

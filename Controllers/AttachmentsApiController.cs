using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasksManager.Services;
using TasksManager.ViewModels;

namespace TasksManager.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AttachmentsApiController : ControllerBase
    {
        private readonly IAttachmentService _attachmentService;

        public AttachmentsApiController(IAttachmentService attachmentService)
        {
            _attachmentService = attachmentService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] AttachmentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _attachmentService.Create(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _attachmentService.GetById(id);
            return Ok(result);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetAllFilter(string? sortOrder, string? currentFilter, string? searchString, int? taskId, int? pageNumber, int pageSize = 3)
        {
            var result = await _attachmentService.GetAllFilter(sortOrder!, currentFilter!, searchString!, taskId, pageNumber, pageSize);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _attachmentService.Delete(id);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] AttachmentViewModel request)
        {
            var result = await _attachmentService.Update(request);
            return Ok(result);
        }
    }
}
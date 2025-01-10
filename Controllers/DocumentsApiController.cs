
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasksManager.Services;
using TasksManager.ViewModels;

namespace TasksManager.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class DocumentsApiController : ControllerBase
    {
        private readonly IDocumentService _documentService;

        public DocumentsApiController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] DocumentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _documentService.Create(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _documentService.GetById(id);
            return Ok(result);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetAllFilter(string? sortOrder, string? currentFilter, string? searchString, int? taskId, int? pageNumber, int pageSize = 3)
        {
            var result = await _documentService.GetAllFilter(sortOrder!, currentFilter!, searchString!, taskId, pageNumber, pageSize);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _documentService.Delete(id);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] DocumentViewModel request)
        {
            var result = await _documentService.Update(request);
            return Ok(result);
        }
    }
}
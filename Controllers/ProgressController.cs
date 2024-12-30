using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TasksManager.Services;
using TasksManager.ViewModels;

namespace TasksManager.Controllers
{
    public class ProgressController : Controller
    {
        private readonly IProgressService _progressService;
        private readonly ITasksService _tasksService;

        int PAGESIZE = 3;

        public ProgressController(IProgressService progressService, ITasksService tasksService)
        {
            _progressService = progressService;
            _tasksService = tasksService;
        }

        // GET: Progress
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? taskId, int? pageNumber)
        {
            ViewData["ProgressSortParm"] = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("progress") ? "progress_desc" : "";
            ViewData["NoteSortParm"] = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("note") ? "note_desc" : "note";

            var progress = await _tasksService.GetAll();
            ViewData["TaskId"] = progress.Select(x => new SelectListItem()
            {
                Text = x.Task_Name,
                Value = x.Task_Id.ToString(),
                Selected = taskId.HasValue && taskId == x.Task_Id
            });
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentSort"] = sortOrder;

            return View(await _progressService.GetAllFilter(sortOrder, currentFilter, searchString, taskId, pageNumber, PAGESIZE));
        }

        // GET: Progress/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var progress = await _progressService.GetById(id);
            if (progress == null)
            {
                return NotFound();
            }
            return View(progress);
        }

        // GET: Progress/Create
        public async Task<IActionResult> Create()
        {
            ViewData["TaskId"] = new SelectList(await _tasksService.GetAll(), "Task_Id", "Task_Name");
            return View();
        }

        // POST: Progress/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProgressRequest request)
        {
            if (ModelState.IsValid)
            {
                await _progressService.Create(request);
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }

        // GET: Progress/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var progress = await _progressService.GetById(id);
            if (progress == null)
            {
                return NotFound();
            }
            ViewData["TaskId"] = new SelectList(await _tasksService.GetAll(), "Task_Id", "Task_Name");

            return View(progress);
        }

        // POST: Progress/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProgressViewModel progress)
        {
            if (id != progress.Progress_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _progressService.Update(progress);
                return RedirectToAction(nameof(Index));           
            }
            return View(progress);
        }

        // GET: Progress/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var progress = await _progressService.GetById(id);
            if (progress == null)
            {
                return NotFound();
            }

            return View(progress);
        }

        // POST: Progress/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _progressService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

    }
}

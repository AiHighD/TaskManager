using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using TasksManager.Data;
using TasksManager.Data.Entities;
using TasksManager.ViewModels;
using TasksManager.Services;
using Microsoft.AspNetCore.Authorization;
using TasksManager.Filters;

namespace TasksManager.Controllers
{
    [TypeFilter(typeof(AuthenticationFilter))]
    public class TasksController : Controller
    {
        private readonly ITasksService _tasksService;
        // private readonly TasksDbContext _context;
        // private readonly IMapper _mapper;
        int PAGESIZE = 3;
        public TasksController(ITasksService tasksService)
        {
            _tasksService = tasksService;
        }

        // GET: Tasks
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("task") ? "task_desc" : "";
            ViewData["TopicSortParm"] = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("topic") ? "topic_desc" : "topic";
            ViewData["DescriptionSortParm"] = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("description") ? "description_desc" : "description";
            ViewData["StatusSortParm"] = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("status") ? "status_desc" : "status";
            ViewData["StartDateSortParm"] = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("start_time") ? "start_time_desc" : "";
            ViewData["EndDateSortParm"] = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("end_time") ? "end_time_desc" : "";

            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentSort"] = sortOrder;
            return View(await _tasksService.GetAllFilter(sortOrder, currentFilter, searchString, pageNumber, PAGESIZE));
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var tasks = await _tasksService.GetById(id);
            if (tasks == null)
            {
                return NotFound();
            }
            return View(tasks);
        }

        // GET: Tasks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskRequest request)
        {
            if (ModelState.IsValid)
            {
                await _tasksService.Create(request);
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var tasks = await _tasksService.GetById(id);
            if (tasks == null)
            {
                return NotFound();
            }
            return View(tasks);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TaskViewModel tasks)
        {
            if (id != tasks.Task_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _tasksService.Update(tasks);
                return RedirectToAction(nameof(Index));
            }
            return View(tasks);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var tasks = await _tasksService.GetById(id);
            if (tasks == null)
            {
                return NotFound();
            }
            return View(tasks);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _tasksService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

    }
}

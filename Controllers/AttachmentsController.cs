using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TasksManager.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TasksManager.Data;
using TasksManager.Data.Entities;
using TasksManager.Services;
using TasksManager.ViewModels;

namespace TasksManager.Controllers
{
    [TypeFilter(typeof(AuthenticationFilter))]
    public class AttachmentsController : Controller
    {
        private readonly ITasksService _tasksService;
        private readonly IAttachmentService _attachmentService;

        int PAGESIZE = 3;

        public AttachmentsController(IAttachmentService attachmentService, ITasksService tasksService)
        {
            _attachmentService = attachmentService;
            _tasksService = tasksService;
        }

        // GET: Attanhments
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? taskId, int? pageNumber)
        {
            ViewData["DescriptionSortParm"] = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("description") ? "description_desc" : "description";
            ViewData["DateSortParm"] = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("date_created") ? "date_created_desc" : "";

            var tasks = await _tasksService.GetAll();
            ViewData["TaskId"] = tasks.Select(t => new SelectListItem()
            {
                Text = t.Task_Name,
                Value = t.Task_Id.ToString(),
                Selected = taskId.HasValue && taskId == t.Task_Id
            });
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentSort"] = sortOrder;
            return View(await _attachmentService.GetAllFilter(sortOrder, currentFilter, searchString, taskId, pageNumber, PAGESIZE));
        }


        // GET: Attanhments/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var attachments = await _attachmentService.GetById(id);
            if (attachments == null)
            {
                return NotFound();
            }

            return View(attachments);
        }

        // GET: Attanhments/Create
        public async Task<IActionResult> Create()
        {
            ViewData["TaskId"] = new SelectList(await _tasksService.GetAll(), "Task_Id", "Task_Name");
            return View();
        }

        // POST: Attanhments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AttachmentRequest request)
        {
            if (ModelState.IsValid)
            {
                await _attachmentService.Create(request);
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }

        // GET: Attanhments/Edit/5
        public async Task<IActionResult> Edit(int id)
        {

            var attachments = await _attachmentService.GetById(id);
            if (attachments == null)
            {
                return NotFound();
            }
            ViewData["TaskId"] = new SelectList(await _tasksService.GetAll(), "Task_Id", "Task_Name");
            return View(attachments);
        }

        // POST: Attanhments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AttachmentViewModel attachments)
        {
            if (id != attachments.Attachments_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _attachmentService.Update(attachments);
                return RedirectToAction(nameof(Index));
            }
            return View(attachments);
        }

        // GET: Attanhments/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var attachments = await _attachmentService.GetById(id);
            if (attachments == null)
            {
                return NotFound();
            }
            return View(attachments);
        }

        // POST: Attanhments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _attachmentService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

    }
}

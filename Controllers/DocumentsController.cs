using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
    // [Authorize]
    public class DocumentsController : Controller
    {

        private readonly IDocumentService _documentService;
        private readonly ITasksService _tasksService;

        int PAGESIZE = 5;

        public DocumentsController(IDocumentService documentService, ITasksService tasksService)
        {
            _documentService = documentService;
            _tasksService = tasksService;
        }

        // GET: Documents
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? taskId, int? pageNumber)
        {
            ViewData["DocSortParm"] = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("doc") ? "doc_desc" : "doc";
            ViewData["DateSortParm"] = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("date_created") ? "date_created_desc" : "date_created";

            var documents = await _tasksService.GetAll();
            ViewData["TaskId"] = documents.Select(d => new SelectListItem()
            {
                Text = d.Task_Name,
                Value = d.Task_Id.ToString(),
                Selected = taskId.HasValue && d.Task_Id == taskId
            });
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentFilter"] = searchString;

            return View(await _documentService.GetAllFilter(sortOrder, currentFilter, searchString, taskId, pageNumber, PAGESIZE));
        }

        // GET: Documents/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var documents = await _documentService.GetById(id);
            if (documents == null)
            {
                return NotFound();
            }

            return View(documents);
        }

        // GET: Documents/Create
        public async Task<IActionResult> Create()
        {
            ViewData["TaskId"] = new SelectList(await _tasksService.GetAll(), "Task_Id", "Task_Name");
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DocumentRequest request)
        {
            if (ModelState.IsValid)
            {
                await _documentService.Create(request);
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }

        // GET: Documents/Edit/5
        public async Task<IActionResult> Edit(int id)
        {

            var documents = await _documentService.GetById(id);
            if (documents == null)
            {
                return NotFound();
            }
            ViewData["TaskId"] = new SelectList(await _tasksService.GetAll(), "Task_Id", "Task_Name");
            return View(documents);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DocumentViewModel documents)
        {
            if (id != documents.Documents_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _documentService.Update(documents);
                return RedirectToAction(nameof(Index));
            }
            return View(documents);
        }

        // GET: Documents/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var documents = await _documentService.GetById(id);
            if (documents == null)
            {
                return NotFound();
            }

            return View(documents);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _documentService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

    }
}

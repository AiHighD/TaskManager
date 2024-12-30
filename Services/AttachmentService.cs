using System.Drawing.Printing;
using System.Net.Http.Headers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TasksManager.Data;
using TasksManager.Data.Entities;
using TasksManager.ViewModels;


namespace TasksManager.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly TasksDbContext _context;
        private readonly IMapper _mapper;
        private const string UPLOAD_FOLDER = "uploads";

        public AttachmentService(TasksDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<string> SaveFile(IFormFile file)
        {

            var allowedExtensions = new[] {
            ".jpg", ".jpeg", ".png", ".gif", ".pdf",
            ".docx", ".txt", ".xlsx", ".pptx", ".zip", ".PDF"
            };

            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
                throw new FileTypeNotSupportedException($"File type {fileExtension} is not supported");

            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", UPLOAD_FOLDER);

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var filePath = Path.Combine(uploadPath, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/{UPLOAD_FOLDER}/{fileName}";
        }

        public class FileTypeNotSupportedException : Exception
        {
            public FileTypeNotSupportedException(string message) : base(message) { }
        }

        public async Task<int> Create(AttachmentRequest request)
        {
            var attach = _mapper.Map<Attachments>(request);

            if (request.FileUpload != null)
                attach.File = await SaveFile(request.FileUpload);

            if (request.ImageUpload != null)
                attach.Image = await SaveFile(request.ImageUpload);

            _context.Attachments.Add(attach);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int id)
        {
            var attach = await _context.Attachments.FindAsync(id);
            if (attach == null) return 0;

            if (!string.IsNullOrEmpty(attach.Image))
                DeleteFile(attach.Image);

            if (!string.IsNullOrEmpty(attach.File))
                DeleteFile(attach.File);

            _context.Attachments.Remove(attach);
            return await _context.SaveChangesAsync();
        }

        private void DeleteFile(string filePath)
        {
            var fullPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                filePath.TrimStart('/')
            );
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

        public async Task<PaginatedList<AttachmentViewModel>> GetAllFilter(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? taskId,
            int? pageNumber,
            int pageSize)
        {
            if (searchString != null)
                pageNumber = 1;
            else
                searchString = currentFilter;

            var query = _context.Attachments.AsQueryable();

            if (taskId.HasValue)
                query = query.Where(s => s.TaskId == taskId);

            if (!string.IsNullOrEmpty(searchString))
                query = query.Where(s => s.Description != null && s.Description.Contains(searchString));

            query = sortOrder switch
            {
                "description_desc" => query.OrderByDescending(s => s.Description),
                "date_created" => query.OrderBy(s => s.Time_Create_Tep),
                "date_created_desc" => query.OrderByDescending(s => s.Time_Create_Tep),
                _ => query.OrderBy(s => s.Description)
            };

            var items = await query.ToListAsync();
            var viewModels = _mapper.Map<IEnumerable<AttachmentViewModel>>(items);

            return PaginatedList<AttachmentViewModel>.Create(
                viewModels,
                pageNumber ?? 1,
                pageSize
            );
        }

        public async Task<AttachmentViewModel> GetById(int id)
        {
            var attach = await _context.Attachments
                .FirstOrDefaultAsync(m => m.Attachments_Id == id);
            return _mapper.Map<AttachmentViewModel>(attach);
        }

        public async Task<int> Update(AttachmentViewModel request)
        {
            if (!AttachmentExists(request.Attachments_Id))
                throw new Exception("Attachment does not exist");

            var attachment = await _context.Attachments
                .FindAsync(request.Attachments_Id);

            if (attachment == null)
                throw new Exception($"Attachment with ID {request.Attachments_Id} not found");

            if (request.ImageUpload != null)
            {
                if (!string.IsNullOrEmpty(attachment.Image))
                    DeleteFile(attachment.Image);
                attachment.Image = await SaveFile(request.ImageUpload);
            }

            if (request.FileUpload != null)
            {
                if (!string.IsNullOrEmpty(attachment.File))
                    DeleteFile(attachment.File);
                attachment.File = await SaveFile(request.FileUpload);
            }

            attachment.Description = request.Description;
            attachment.TaskId = request.TaskId;
            attachment.Time_Create_Tep = request.Time_Create_Tep;

            _context.Update(attachment);
            return await _context.SaveChangesAsync();
        }

        private bool AttachmentExists(int id)
        {
            return _context.Attachments.Any(e => e.Attachments_Id == id);
        }
    }
}
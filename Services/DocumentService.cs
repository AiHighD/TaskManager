using System.Reflection.Metadata;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TasksManager.Data;
using TasksManager.ViewModels;
using TasksManager.Data.Entities;

namespace TasksManager.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly TasksDbContext _context;
        private readonly IMapper _mapper;

        public DocumentService(TasksDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Create(DocumentRequest request)
        {
            var document = _mapper.Map<Documents>(request);
            _context.Documents.Add(document);
            
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(DocumentViewModel request)
        {
            var documents = await _context.Documents
                .FindAsync(request.Documents_Id);

            if (!DocumentExists(request.Documents_Id))
            {
                throw new Exception("Document not found");
            }

            if (documents == null)
            {
                throw new Exception("Document not found");
            }

            documents.Doc = request.Doc;
            documents.TaskId = request.TaskId;
            documents.Time_Create_Doc = request.Time_Create_Doc;

            _context.Update(documents);
            return await _context.SaveChangesAsync();
        }

        private bool DocumentExists(int documents_Id)
        {
            return _context.Documents.Any(e => e.Documents_Id == documents_Id);
        }

        public async Task<int> Delete(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document != null)
            {
                _context.Documents.Remove(document);
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<PaginatedList<DocumentViewModel>> GetAllFilter(string sortOrder, string currentFilter, string searchString, int? taskId, int? pageNumber, int pageSize)
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var documents = from m in _context.Documents select m;

            if (taskId != null)
            {
                documents = documents.Where(s => s.TaskId == taskId);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                documents = documents.Where(s => s.Doc!.Contains(searchString));
            }

            documents = sortOrder switch
            {
                "doc_desc" => documents.OrderByDescending(s => s.Doc),
                "date_created" => documents.OrderBy(s => s.Time_Create_Doc),
                "date_created_desc" => documents.OrderByDescending(s => s.Time_Create_Doc),
                _ => documents.OrderBy(s => s.Doc),
            };

            return PaginatedList<DocumentViewModel>.Create(_mapper.Map<IEnumerable<DocumentViewModel>>(await documents.ToListAsync()), pageNumber ?? 1, pageSize);
        }

        public async Task<DocumentViewModel> GetById(int id)
        {
            var document = await _context.Documents.FirstOrDefaultAsync(m => m.Documents_Id == id);
            return _mapper.Map<DocumentViewModel>(document);
        }

    }
}
using TasksManager.ViewModels;

namespace TasksManager.Services
{
    public interface IDocumentService
    {
        Task<PaginatedList<DocumentViewModel>> GetAllFilter(string sortOrder, string currentFilter, string searchString, int? taskId, int? pageNumber, int pageSize);
        Task<DocumentViewModel> GetById(int id);
        Task<int> Create(DocumentRequest document);
        Task<int> Update(DocumentViewModel document);
        Task<int> Delete(int id);
    }
}
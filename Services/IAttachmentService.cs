using TasksManager.ViewModels;

namespace TasksManager.Services
{

    public interface IAttachmentService

    {
        
        Task<PaginatedList<AttachmentViewModel>> GetAllFilter(string sortOrder, string currentFilter, string searchString,int? taskId ,int? pageNumber, int pageSize);

        Task<AttachmentViewModel> GetById(int id);

        Task<int> Create(AttachmentRequest request);
        Task<int> Delete(int id);
        Task<int> Update(AttachmentViewModel request);

    }

}
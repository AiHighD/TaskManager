using TasksManager.ViewModels;

namespace TasksManager.Services
{

    public interface IProgressService

    {

        Task<PaginatedList<ProgressViewModel>> GetAllFilter(string sortOrder, string currentFilter, string searchString, int? taskId, int? pageNumber, int pageSize);

        Task<ProgressViewModel> GetById(int id);

        Task<int> Create(ProgressRequest progress);
        Task<int> Delete(int id);
        Task<int> Update(ProgressViewModel progress);

    }

}
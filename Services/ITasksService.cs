using TasksManager.ViewModels;

namespace TasksManager.Services
{
    public interface ITasksService
    {   
        Task<IEnumerable<TaskViewModel>> GetAll();
        Task<PaginatedList<TaskViewModel>> GetAllFilter(string sortOrder, string currentFilter, string searchString, int? pageNumber, int pageSize);
        Task<TaskViewModel> GetById(int id);
        Task<int> Create(TaskRequest request);
        Task<int> Update(TaskViewModel request);
        Task<int> Delete(int id);
    }
}
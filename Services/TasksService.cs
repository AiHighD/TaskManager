using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TasksManager.Data;
using TasksManager.Data.Entities;
using TasksManager.ViewModels;

namespace TasksManager.Services
{
    public class TasksService : ITasksService
    {
        private readonly TasksDbContext _context;
        private readonly IMapper _mapper;

        public TasksService(TasksDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Create(TaskRequest request)
        {
            var tasks = _mapper.Map<Tasks>(request);
            _context.Add(tasks);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int id)
        {
            var tasks = await _context.Tasks.FindAsync(id);
            if (tasks != null)
            {
                _context.Tasks.Remove(tasks);
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TaskViewModel>> GetAll()
        {
            var products = await _context.Tasks.ToListAsync();
            return _mapper.Map<IEnumerable<TaskViewModel>>(products);
        }
        public async Task<PaginatedList<TaskViewModel>> GetAllFilter(string sortOrder, string currentFilter, string searchString, int? pageNumber, int pageSize)
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var tasks = from m in _context.Tasks select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                tasks = tasks.Where(s => s.Task_Name!.Contains(searchString)
                || s.Topic!.Contains(searchString)
                || s.Description!.Contains(searchString)
                || s.Status!.Contains(searchString));
            }

            tasks = sortOrder switch
            {
                "task_desc" => tasks.OrderByDescending(s => s.Task_Name),
                "topic" => tasks.OrderBy(s => s.Topic),
                "topic_desc" => tasks.OrderByDescending(s => s.Topic),
                "description" => tasks.OrderBy(s => s.Description),
                "description_desc" => tasks.OrderByDescending(s => s.Description),
                "status" => tasks.OrderBy(s => s.Status),
                "status_desc" => tasks.OrderByDescending(s => s.Status),
                "start_time" => tasks.OrderBy(s => s.StartTime),
                "start_time_desc" => tasks.OrderByDescending(s => s.StartTime),
                "end_time" => tasks.OrderBy(s => s.EndTime),
                "end_time_desc" => tasks.OrderByDescending(s => s.EndTime),
                _ => tasks.OrderBy(s => s.Task_Name),
            };

            return PaginatedList<TaskViewModel>.Create(_mapper.Map<IEnumerable<TaskViewModel>>(await tasks.ToListAsync()), pageNumber ?? 1, pageSize);
        }

        public async Task<TaskViewModel> GetById(int id)
        {
            var tasks = await _context.Tasks
                .FirstOrDefaultAsync(m => m.Task_Id == id);
            return _mapper.Map<TaskViewModel>(tasks);
        }

        public async Task<int> Update(TaskViewModel request)
        {
            if (!CourseExists(request.Task_Id))
            {
                throw new Exception("Task does not exist");
            }
            _context.Update(_mapper.Map<Tasks>(request));
            return await _context.SaveChangesAsync();
        }

        private bool CourseExists(int id)
        {
            return _context.Tasks.Any(e => e.Task_Id == id);
        }
    }
}
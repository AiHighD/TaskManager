using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TasksManager.Data;
using TasksManager.Data.Entities;
using TasksManager.ViewModels;

namespace TasksManager.Services
{
    public class ProgressService : IProgressService
    {
        private readonly TasksDbContext _context;
        private readonly IMapper _mapper;

        public ProgressService(TasksDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<int> Create(ProgressRequest request)
        {
            var progress = _mapper.Map<Progress>(request);
            _context.Add(progress);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int id)
        {
            var progress = await _context.Progress.FindAsync(id);
            if (progress != null)
            {
                _context.Progress.Remove(progress);
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<PaginatedList<ProgressViewModel>> GetAllFilter(string sortOrder, string currentFilter, string searchString, int? taskId, int? pageNumber, int pageSize)
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var progress = from m in _context.Progress select m;

            if (taskId != null)
            {
                progress = progress.Where(s => s.TaskId == taskId);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                progress = progress.Where(s => s.Progress_Percentage!.ToString().Contains(searchString)
                || s.Note!.Contains(searchString));
            }

            progress = sortOrder switch
            {
                "progress_desc" => progress.OrderByDescending(s => s.Progress_Percentage),
                "note" => progress.OrderBy(s => s.Note),
                "note_desc" => progress.OrderByDescending(s => s.Note),
                _ => progress.OrderBy(s => s.Progress_Percentage),
            };

            return PaginatedList<ProgressViewModel>.Create(_mapper.Map<IEnumerable<ProgressViewModel>>(await progress.ToListAsync()), pageNumber ?? 1, pageSize);
        }


        public async Task<ProgressViewModel> GetById(int id)
        {
            var progress = await _context.Progress
            .FirstOrDefaultAsync(m => m.Progress_Id == id);
            return _mapper.Map<ProgressViewModel>(progress);
        }

        public async Task<int> Update(ProgressViewModel request)
        {
            var progress = await _context.Progress.FindAsync(request.Progress_Id);

            if (!ProgressExists(request.Progress_Id))
            {
                throw new Exception("Progress not found");
            }

            if (progress == null)
            {
                throw new Exception("Progress not found");
            }

            progress.Progress_Percentage = request.Progress_Percentage;
            progress.Note = request.Note;
            progress.TaskId = request.TaskId;

            _context.Update(progress);
            return await _context.SaveChangesAsync();
        }

        private bool ProgressExists(int progress_Id)
        {
            return _context.Progress.Any(e => e.Progress_Id == progress_Id);
        }

    }
}
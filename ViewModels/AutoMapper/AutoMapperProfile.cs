using AutoMapper;
using TasksManager.Data.Entities;

namespace TasksManager.ViewModels.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        
        public AutoMapperProfile()
        {
            // Task
            CreateMap<Tasks, TaskViewModel>();
            CreateMap<TaskViewModel, Tasks>();
            CreateMap<TaskRequest, Tasks>();

            // Proress
            CreateMap<Progress, ProgressViewModel>();
            CreateMap<ProgressViewModel, Progress>();
            CreateMap<ProgressRequest, Progress>();

            // Document
            CreateMap<Documents, DocumentViewModel>();
            CreateMap<DocumentViewModel, Documents>();
            CreateMap<DocumentRequest, Documents>();

            // Attachment
            CreateMap<Attachments, AttachmentViewModel>()
           .ForMember(dest => dest.Task, opt => opt.MapFrom(src => src.TaskId));

            CreateMap<AttachmentViewModel, Attachments>()
                .ForMember(dest => dest.TaskId, opt => opt.MapFrom(src => src.TaskId));

            CreateMap<AttachmentRequest, Attachments>()
                .ForMember(dest => dest.TaskId, opt => opt.MapFrom(src => src.TaskId));
        }       
    }
}
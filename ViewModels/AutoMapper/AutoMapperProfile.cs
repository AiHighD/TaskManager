using AutoMapper;
using TasksManager.Data.Entities;
using TasksManager.ViewModels;

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
            CreateMap<Progress, ProgressViewModel>()
                .ForMember(dest => dest.Task, opt => opt.MapFrom(src => src.TaskId)); // Task ánh xạ từ TaskId

            CreateMap<ProgressViewModel, Progress>()
                .ForMember(dest => dest.TaskId, opt => opt.MapFrom(src => src.TaskId)); // TaskId ánh xạ từ Task

            CreateMap<ProgressRequest, Progress>()
                .ForMember(dest => dest.TaskId, opt => opt.MapFrom(src => src.TaskId));

            // Document
            CreateMap<Documents, DocumentViewModel>()
                .ForMember(dest => dest.Task, opt => opt.MapFrom(src => src.TaskId)); // Task ánh xạ từ TaskId

            CreateMap<DocumentViewModel, Documents>()
                .ForMember(dest => dest.TaskId, opt => opt.MapFrom(src => src.TaskId)); // TaskId ánh xạ từ Task

            CreateMap<DocumentRequest, Documents>()
                .ForMember(dest => dest.TaskId, opt => opt.MapFrom(src => src.TaskId));


            // Attachment
            CreateMap<Attachments, AttachmentViewModel>()
                .ForMember(dest => dest.Task, opt => opt.MapFrom(src => src.TaskId));

            CreateMap<AttachmentViewModel, Attachments>()
                .ForMember(dest => dest.TaskId, opt => opt.MapFrom(src => src.TaskId));

            CreateMap<AttachmentRequest, Attachments>()
                .ForMember(dest => dest.TaskId, opt => opt.MapFrom(src => src.TaskId));

            //user
            CreateMap<User, UserViewModel>();
            CreateMap<RegisterRequest, User>()
            .ForMember(des => des.UserName, opt => opt.MapFrom(src => src.Email));
        }
    }
}
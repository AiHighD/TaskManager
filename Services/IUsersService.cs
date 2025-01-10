using TasksManager.ViewModels;

namespace TasksManager.Services
{
    public interface IUsersService
    {
        Task<string> Authenticate(LoginRequest request);

        Task<bool> Register(RegisterRequest request);
    }
}
using APIs.Contracts;
using APIs;
using DTOs.Contracts;

namespace BLL.Services.Interface
{
    public interface IUserService
    {
        Task<string> CreateUser(CreateUserRequest user);

        Task<LoginResponse> Login(LoginRequest loginRequest);

        Task<Role> GetRole(string email);

        Task<IEnumerable<User>> GetInactiveUsers(DateTime dateTime);

        Task LogUserActivity(string email);
    }
}

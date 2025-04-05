using APIs;

namespace APIs.Repository.Interface
{
    public interface IUserRepository
    {
        Task<User> CreateUser(User user);

        Task<User> GetUserByEmailAsync(string email);

        Task<IEnumerable<User>> GetInactiveUsersAsync(DateTime dateTime);

        Task LogUserActivity(string email);

        Task<Role> GetRole(string email);
    }
}

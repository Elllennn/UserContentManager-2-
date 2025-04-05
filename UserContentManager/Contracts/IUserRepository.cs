using UserContentManager.Models;

namespace UserContentManager.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(string userId);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
    }
}

using UserContentManager.Models;

namespace UserContentManager.Contracts
{
    public interface IUserService
    {
        Task<List<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(string userId);
        Task AddUserAsync(User newUser);
        Task UpdateUserAsync(User user);
    }
}

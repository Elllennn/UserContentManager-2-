using System.Text.Json;
using Newtonsoft.Json;
using UserContentManager.Models;

namespace UserContentManager.Repositories
{
    public class UserRepository:IUserRepository
    {
        private readonly string _filePath = "data\\users.json";


        public async Task<List<User>> GetUsersAsync()
        {
            if (!File.Exists(_filePath))
                return new List<User>();

            var jsonString = await File.ReadAllTextAsync(_filePath); return JsonConvert.DeserializeObject<List<User>>(jsonString) ?? new List<User>();
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            var users = await GetUsersAsync();
            return users.FirstOrDefault(u => u.Id == userId);
        }

        public async Task AddUserAsync(User user)
        {
            var users = await GetUsersAsync();
            users.Add(user);
            await SaveUsersAsync(users);
        }

        public async Task UpdateUserAsync(User user)
        {
            var users = await GetUsersAsync();
            var existingUser = users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                existingUser.Email = user.Email;
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Avatar = user.Avatar;
                await SaveUsersAsync(users);
            }
        }

        private async Task SaveUsersAsync(List<User> users)
        {
            var jsonString = JsonConvert.SerializeObject(users);
            await File.WriteAllTextAsync(_filePath, jsonString);
        }

    }
}

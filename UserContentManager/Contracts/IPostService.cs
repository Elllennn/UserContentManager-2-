using UserContentManager.Models;

namespace UserContentManager.Contracts
{
    public interface IPostService
    {
        Task<List<Post>> GetPostsByUserIdAndTitleAsync(int userId, string title);
        Task<Post> GetPostByIdAsync(int id);
        Task AddPostAsync(Post newPost);
        Task DeletePostAsync(int id);
        Task UpdateUserAsync(User user);
    }
}

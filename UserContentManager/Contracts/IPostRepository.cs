using UserContentManager.Models;

namespace UserContentManager.Repositories
{
    public interface IPostRepository
    {
        Task<List<Post>> GetPostsAsync();
        Task<Post> GetPostByIdAsync(int id);
        Task<List<Post>> GetPostsByUserIdAndTitleAsync(int userId, string title);
        Task AddPostAsync(Post post);
        Task DeletePostAsync(int id);
    }
}

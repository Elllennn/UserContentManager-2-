using System.Text.Json;
using Newtonsoft.Json;
using UserContentManager.Models;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace UserContentManager.Repositories
{
    public class PostRepository:IPostRepository
    {
        private readonly string _filePath ="data\\posts.json";
        
        public async Task<List<Post>> GetPostsAsync()
        {
            if (!File.Exists(_filePath))
                return new List<Post>();

            var jsonString = await File.ReadAllTextAsync(_filePath);
            return JsonConvert.DeserializeObject<List<Post>>(jsonString) ?? new List<Post>();
        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
            var posts = await GetPostsAsync();
            return posts.FirstOrDefault(p => p.Id == id);
        }

        public async Task<List<Post>> GetPostsByUserIdAndTitleAsync(int userId, string title)
        {
            var posts = await GetPostsAsync();
            return posts.Where(p => p.UserId == userId && p.Title.Contains(title)).ToList();
        }

        public async Task AddPostAsync(Post post)
        {
            var posts = await GetPostsAsync();
            post.Id = posts.Any() ? posts.Max(p => p.Id) + 1 : 1;
            posts.Add(post);
            await SavePostsAsync(posts);
        }

        public async Task DeletePostAsync(int id)
        {
            var posts = await GetPostsAsync();
            var post = posts.FirstOrDefault(p => p.Id == id);
            if (post != null)
            {
                posts.Remove(post);
                await SavePostsAsync(posts);
            }
        }

        private async Task SavePostsAsync(List<Post> posts)
        {
            var jsonString = JsonConvert.SerializeObject(posts);
            await File.WriteAllTextAsync(_filePath, jsonString);
        }

    }
}

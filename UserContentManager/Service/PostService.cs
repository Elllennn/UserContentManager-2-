using Microsoft.AspNetCore.Mvc;
using UserContentManager.Models;
using UserContentManager.Repositories;

namespace UserContentManager.Service
{
    public class PostService
    {

        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;

        public PostService(IPostRepository postRepository, IUserRepository userRepository)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
        }

        public async Task<List<Post>> GetPostsByUserIdAndTitleAsync(int userId, string title)
        {
            return await _postRepository.GetPostsByUserIdAndTitleAsync(userId, title);
        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
            return await _postRepository.GetPostByIdAsync(id);
        }

        public async Task AddPostAsync(Post newPost)
        {
            var user = await _userRepository.GetUserByIdAsync(newPost.UserId.ToString());
            if (user == null)
            {
                var newUser = new User
                {
                    Id = newPost.UserId.ToString(),
                    Email = "newuser@example.com",
                    FirstName = "John",
                    LastName = "Doe",
                    Avatar = "https://example.com/avatar.jpg"
                };
                await _userRepository.AddUserAsync(newUser);
            }
            await _postRepository.AddPostAsync(newPost);
        }

        public async Task DeletePostAsync(int id)
        {
            await _postRepository.DeletePostAsync(id);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateUserAsync(user);
        }
    }


}

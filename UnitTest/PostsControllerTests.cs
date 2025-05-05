using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserContentManager.Models;
using UserContentManager.Repositories;
using UserContentManager.Service;

namespace UnitTest
{
    public class PostsControllerTests
    {
        private readonly Mock<IPostRepository> _mockPostRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly PostService _postService;

        public PostsControllerTests()
        {
            _mockPostRepository = new Mock<IPostRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _postService = new PostService(_mockPostRepository.Object, _mockUserRepository.Object);
        }

        [Fact]
        public async Task GetPostsByUserIdAndTitleAsync_ReturnsPostList()
        {
            var userId = 1;
            var title = "Test Title";
            var expectedPosts = new List<Post>
            {
                new Post { Id = 1, UserId = userId, Title = title }
            };

            _mockPostRepository.Setup(repo =>
                repo.GetPostsByUserIdAndTitleAsync(userId, title))
                .ReturnsAsync(expectedPosts);

            var result = await _postService.GetPostsByUserIdAndTitleAsync(userId, title);

            Assert.Equal(expectedPosts, result);
            _mockPostRepository.Verify(repo =>
                repo.GetPostsByUserIdAndTitleAsync(userId, title), Times.Once);
        }

        [Fact]
        public async Task GetPostByIdAsync_ReturnsPost()
        {
            var postId = 1;
            var expectedPost = new Post { Id = postId, Title = "Test Post" };

            _mockPostRepository.Setup(repo =>
                repo.GetPostByIdAsync(postId))
                .ReturnsAsync(expectedPost);

            var result = await _postService.GetPostByIdAsync(postId);

            Assert.Equal(expectedPost, result);
            _mockPostRepository.Verify(repo =>
                repo.GetPostByIdAsync(postId), Times.Once);
        }

        [Fact]
        public async Task AddPostAsync_WithExistingUser_CallsAddPostAsync()
        {
            var userId = 1;
            var newPost = new Post { Id = 1, UserId = userId, Title = "New Post" };
            var existingUser = new User { Id = userId.ToString() };

            _mockUserRepository.Setup(repo =>
                repo.GetUserByIdAsync(userId.ToString()))
                .ReturnsAsync(existingUser);

            await _postService.AddPostAsync(newPost);

            _mockPostRepository.Verify(repo =>
                repo.AddPostAsync(newPost), Times.Once);
            _mockUserRepository.Verify(repo =>
                repo.AddUserAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task AddPostAsync_WithNewUser_CreatesUserAndAddsPost()
        {
            var userId = 1;
            var newPost = new Post { Id = 1, UserId = userId, Title = "New Post" };

            _mockUserRepository.Setup(repo =>
                repo.GetUserByIdAsync(userId.ToString()))
                .ReturnsAsync((User)null);

            await _postService.AddPostAsync(newPost);

            _mockPostRepository.Verify(repo =>
                repo.AddPostAsync(newPost), Times.Once);
            _mockUserRepository.Verify(repo =>
                repo.AddUserAsync(It.Is<User>(u =>
                    u.Id == userId.ToString() &&
                    u.Email == "newuser@example.com" &&
                    u.FirstName == "John" &&
                    u.LastName == "Doe")),
                Times.Once);
        }

        [Fact]
        public async Task DeletePostAsync_CallsDeletePostAsync()
        {
            var postId = 1;

            await _postService.DeletePostAsync(postId);

            _mockPostRepository.Verify(repo =>
                repo.DeletePostAsync(postId), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_CallsUpdateUserAsync()
        {
            var user = new User { Id = "1", FirstName = "Updated Name" };

            await _postService.UpdateUserAsync(user);

            _mockUserRepository.Verify(repo =>
                repo.UpdateUserAsync(user), Times.Once);
        }
    }
}

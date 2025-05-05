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
    public class PostServiceTests
    {
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly PostService _postService;

        public PostServiceTests()
        {
            _postRepositoryMock = new Mock<IPostRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _postService = new PostService(_postRepositoryMock.Object, _userRepositoryMock.Object);
        }

        [Fact]
        public async Task GetPostsByUserIdAndTitleAsync_ReturnsPosts_WhenPostsExist()
        {
            var posts = new List<Post> { new Post { Id = 1, UserId = 1, Title = "Test Post" } };
            _postRepositoryMock.Setup(repo => repo.GetPostsByUserIdAndTitleAsync(1, "Test Post")).ReturnsAsync(posts);

            var result = await _postService.GetPostsByUserIdAndTitleAsync(1, "Test Post");

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(1, result[0].Id);
        }

        [Fact]
        public async Task GetPostByIdAsync_ReturnsPost_WhenPostExists()
        {
            var post = new Post { Id = 1, UserId = 1, Title = "Test Post" };
            _postRepositoryMock.Setup(repo => repo.GetPostByIdAsync(1)).ReturnsAsync(post);


            var result = await _postService.GetPostByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Test Post", result.Title);
        }


        [Fact]
        public async Task AddPostAsync_CreatesUser_WhenUserDoesNotExist()
        {
            var newPost = new Post { Id = 1, UserId = 2, Title = "New Post" };
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync("2")).ReturnsAsync((User)null);
            _userRepositoryMock.Setup(repo => repo.AddUserAsync(It.IsAny<User>())).Returns(Task.CompletedTask);
            _postRepositoryMock.Setup(repo => repo.AddPostAsync(It.IsAny<Post>())).Returns(Task.CompletedTask);

            await _postService.AddPostAsync(newPost);

            _userRepositoryMock.Verify(repo => repo.AddUserAsync(It.Is<User>(u => u.Id == "2")), Times.Once);
            _postRepositoryMock.Verify(repo => repo.AddPostAsync(newPost), Times.Once);
        }

        [Fact]
        public async Task AddPostAsync_DoesNotCreateUser_WhenUserExists()
        {

            var newPost = new Post { Id = 1, UserId = 1, Title = "New Post" };
            var existingUser = new User { Id = "1", FirstName = "Existing", LastName = "User" };
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync("1")).ReturnsAsync(existingUser);
            _postRepositoryMock.Setup(repo => repo.AddPostAsync(It.IsAny<Post>())).Returns(Task.CompletedTask);

            await _postService.AddPostAsync(newPost);

            _userRepositoryMock.Verify(repo => repo.AddUserAsync(It.IsAny<User>()), Times.Never);
            _postRepositoryMock.Verify(repo => repo.AddPostAsync(newPost), Times.Once);
        }

        [Fact]
        public async Task DeletePostAsync_CallsDeletePostAsync_WhenPostExists()
        {
            _postRepositoryMock.Setup(repo => repo.DeletePostAsync(1)).Returns(Task.CompletedTask);

            await _postService.DeletePostAsync(1);

            _postRepositoryMock.Verify(repo => repo.DeletePostAsync(1), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_CallsUpdateUserAsync()
        {
            var user = new User { Id = "1", FirstName = "Updated", LastName = "User" };
            _userRepositoryMock.Setup(repo => repo.UpdateUserAsync(user)).Returns(Task.CompletedTask);
            await _postService.UpdateUserAsync(user);

            _userRepositoryMock.Verify(repo => repo.UpdateUserAsync(user), Times.Once);
        }
    }
}

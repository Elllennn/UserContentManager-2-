using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using UserContentManager.Contracts;
using UserContentManager.Controllers;
using UserContentManager.Models;


namespace UnitTest
{
    public class UserControllerTests
        {
            private readonly Mock<IUserService> _mockUserService = new Mock<IUserService>();
            private readonly Mock<IPostService> _mockPostService = new Mock<IPostService>();
            private readonly Mock<IConfiguration> _mockConfiguration = new Mock<IConfiguration>();
            private readonly Mock<ILogger<UserController>> _mockLogger = new Mock<ILogger<UserController>>();
            private HttpClient _httpClient;

            public UserControllerTests()
            {
                _mockConfiguration.Setup(config => config["BaseURLs:UsersURL"]).Returns("https://reqres.in/api/users");
            }

            private UserController SetupController(HttpResponseMessage responseMessage)
            {
                var mockHandler = new Mock<HttpMessageHandler>();
                mockHandler.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.IsAny<HttpRequestMessage>(),
                        ItExpr.IsAny<CancellationToken>()
                    )
                    .ReturnsAsync(responseMessage);

                _httpClient = new HttpClient(mockHandler.Object);
                return new UserController(
                           _mockUserService.Object,
                           _mockPostService.Object,
                           _httpClient,
                           _mockConfiguration.Object,
                           _mockLogger.Object
                       );
            }



            [Fact]
            public async Task GetUsers_ReturnsOk_ForSuccessfulRequest()
            {
                var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("[{\"Id\":1, \"FirstName\": \"Test\"}]", Encoding.UTF8, "application/json")
                };
                var controller = SetupController(mockResponse);
                var result = await controller.GetUsers();
                Assert.IsType<OkObjectResult>(result);
            }
            [Fact]
            public async Task GetUsers_ReturnsBadRequest_ForFailedRequest()
            {
                var mockResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                var controller = SetupController(mockResponse);
                var result = await controller.GetUsers();
                Assert.IsType<BadRequestResult>(result);
            }
            [Fact]
            public async Task AddUser_ReturnsOk_ForSuccessfulAddition()
            {
                var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("[]", Encoding.UTF8, "application/json")
                };

                var controller = SetupController(mockResponse);

                var newUser = new User { Id = "1", FirstName = "New User" };
                var result = await controller.AddUser(newUser);
                Assert.IsType<OkObjectResult>(result);
            }
            [Fact]
            public async Task AddUser_ReturnsBadRequest_ForDuplicateUser()
            {
                var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("[{\"Id\":1, \"FirstName\": \"New User\"}]", Encoding.UTF8, "application/json")
                };

                var controller = SetupController(mockResponse);

                var newUser = new User { Id = "2", FirstName = "New User" };
                var result = await controller.AddUser(newUser);
                Assert.IsType<BadRequestObjectResult>(result);
            }

            [Fact]
            public async Task AddUser_ReturnsBadRequest_OnFailure()
            {
                var mockResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                var controller = SetupController(mockResponse);
                var newUser = new User { Id = "1", FirstName = "Invalid User" };
                var result = await controller.AddUser(newUser);

                Assert.IsType<BadRequestObjectResult>(result);
            }
            [Fact]
            public async Task UpdateUser_ReturnsOk_ForSuccessfulUpdate()
            {
                var mockResponse = new HttpResponseMessage(HttpStatusCode.OK);
                var controller = SetupController(mockResponse);
                var updatedUser = new User { Id = "1", FirstName = "Updated Name" };
                var result = await controller.UpdateUser("1", updatedUser);

                Assert.IsType<OkResult>(result);
            }

            [Fact]
            public async Task UpdateUser_ReturnsBadRequest_OnFailure()
            {
                var mockResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                var controller = SetupController(mockResponse);
                var updatedUser = new User { Id = "1", FirstName = "Invalid Name" };
                var result = await controller.UpdateUser("1", updatedUser);
                Assert.IsType<BadRequestResult>(result);
            }
        }
    }


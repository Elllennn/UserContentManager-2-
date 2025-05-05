using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using UserContentManager.Models;
using UserContentManager.Service;

namespace UserContentManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly PostService _postService;
        private readonly HttpClient _httpClient;
        private readonly ILogger<UserController> _logger;
        private readonly string _userUrl;
        public UserController(UserService userService, PostService postService, HttpClient httpClient, IConfiguration configuration, ILogger<UserController> logger)
        {
            _userService = userService;
            _postService = postService;
            _httpClient = httpClient;
            _logger = logger;
            _userUrl = configuration["BaseURLs:UsersURL"] ?? throw new NullReferenceException("UsersURL in configs is null.");

        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var response = await _httpClient.GetAsync($"{_userUrl}");
            var result = new Post();
            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions();
                options.PropertyNameCaseInsensitive = true;
                var userJson = await response.Content.ReadAsStringAsync();

                var user = JsonConvert.DeserializeObject<List<Post>>(userJson);
                return Ok(user);
            }
            else
            {
                return BadRequest();
            }
        }



        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User newUser)
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(newUser);

                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");


                var result = await _httpClient.GetAsync($"{_userUrl}");
                if (result.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions();
                    options.PropertyNameCaseInsensitive = true;
                    var userJson = await result.Content.ReadAsStringAsync();

                    var users = JsonConvert.DeserializeObject<List<User>>(userJson);
                    foreach(var user in users)
                    {
                        if (newUser.FirstName == user.FirstName)
                            throw new Exception();
                    }
                   
                }
                var response = await _httpClient.PostAsync($"{_userUrl}", content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"{newUser.FirstName} has been added successfully!");
                    return Ok($"Id of New User : {newUser.Id}");
                }

                else
                {
                    _logger.LogInformation("Service is not available");
                    return BadRequest("Service is not available");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("User already exists");
            }
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] User updatedUser)
        {
            var jsonString = JsonConvert.SerializeObject(updatedUser);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_userUrl}/{userId}", content);

            if (response.IsSuccessStatusCode)
            {

                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}

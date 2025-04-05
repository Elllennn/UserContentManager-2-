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
        private readonly string _userUrl;
        public UserController(UserService userService, PostService postService, HttpClient httpClient, IConfiguration configuration)
        {
            _userService = userService;
            _postService = postService;
            _httpClient = httpClient;
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
            var jsonString = JsonConvert.SerializeObject(newUser);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_userUrl}",content);
            var result = new Post();
            if (response.IsSuccessStatusCode)
            {
                return Ok($"Id of New User : {newUser.Id}");
            } 
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] User updatedUser)
        {
            var jsonString = JsonConvert.SerializeObject(updatedUser);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_userUrl}/{userId}",content);
           
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

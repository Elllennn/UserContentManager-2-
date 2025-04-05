using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using UserContentManager.Models;
using UserContentManager.Service;
using static System.Net.Mime.MediaTypeNames;

namespace UserContentManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {



        private readonly PostService _postService;
        private readonly HttpClient _httpClient;
        private readonly string _postUrl;
        public PostsController(PostService postService, HttpClient httpClient, IConfiguration configuration)
        {
            _postService = postService;
            _httpClient = httpClient;
            _postUrl = configuration["BaseURLs:PostsURL"] ?? throw new NullReferenceException("PostURL in configs is null.");

        }

        [HttpGet]
        public async Task<ActionResult<List<Post>>> GetPostsByUserIdAndTitle(int userId, string title)
        {
           
            var response = await _httpClient.GetAsync($"{_postUrl}?userId={userId}&title={title}");
            var result = new Post();
            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions();
                options.PropertyNameCaseInsensitive = true;
                var postJson = await response.Content.ReadAsStringAsync();

                var post = JsonConvert.DeserializeObject<List<Post>>(postJson);
                return Ok(post);
            }
            else
            {
                return BadRequest();
            }
          
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPostById(int id)
        {
            var response = await _httpClient.GetAsync($"{_postUrl}/{id}");
            var result = new Post();
            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions();
                options.PropertyNameCaseInsensitive = true;
                var postJson = await response.Content.ReadAsStringAsync();

                var post = JsonConvert.DeserializeObject<Post>(postJson);
                return Ok(post);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddPost([FromBody] Post newPost)
        {
            var jsonString = JsonConvert.SerializeObject(newPost);
            var content= new StringContent(jsonString, Encoding.UTF8, "application/json");


            var response = await _httpClient.PostAsync($"{_postUrl}",content);
           
            if (response.IsSuccessStatusCode)
            {
               
                return Ok($"Id of New Post:{newPost.Id}");
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_postUrl}/{id}");
            
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


using LearningHttpClient.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LearningHttpClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //api/TodosBasicHttpClient/GetDataUsingHttpClientFactoryBasic
    public class TodosBasicHttpClientController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public TodosBasicHttpClientController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
        }

        [HttpGet]
        [Route("GetDataUsingHttpClientFactoryBasic")]
        public async Task<ActionResult> Index()
        {
            string url = "https://jsonplaceholder.typicode.com/todos/";
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var todos = await response.Content.ReadFromJsonAsync<IEnumerable<Todo>>(_jsonSerializerOptions);
            return Ok(todos);
        }
    }
}

using LearningHttpClient.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LearningHttpClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //api/TodosNamedHttpClient/GetDataUsingNamedHttpClient
    public class TodosNamedHttpClientController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public TodosNamedHttpClientController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
        }

        [HttpGet]
        [Route("GetDataUsingNamedHttpClient")]
        public async Task<ActionResult> Index()
        {
            string endpoint = "/todos";
            var httpClient = _httpClientFactory.CreateClient("todos");
            var response = await httpClient.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var todos = await response.Content.ReadFromJsonAsync<IEnumerable<Todo>>(_jsonSerializerOptions);
            return Ok(todos);
        }
    }
}

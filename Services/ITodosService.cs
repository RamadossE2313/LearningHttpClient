using LearningHttpClient.Models;
using System.Text.Json;

namespace LearningHttpClient.Services
{
    public interface ITodosService
    {
        Task<IEnumerable<Todo>> GetTodos();
    }

    public class TodosService : ITodosService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public TodosService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<IEnumerable<Todo>> GetTodos()
        {
            string endpoint = "/todos";
            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();

            var todos = await response.Content.ReadFromJsonAsync<IEnumerable<Todo>>(_jsonSerializerOptions);

            if(todos == null)
            {
                return new List<Todo>();
            }

            return todos;
        }
    }
}

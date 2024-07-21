using LearningHttpClient.Models;
using Refit;

namespace LearningHttpClient.Services
{
    public interface ITodosClient
    {
        [Get("/todos")]
        Task<IEnumerable<Todo>> GetTodos();
    }
}

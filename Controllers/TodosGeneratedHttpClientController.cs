using LearningHttpClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearningHttpClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //api/TodosGeneratedHttpClient/GetDataUsingGeneratedHttpClient
    public class TodosGeneratedHttpClientController : ControllerBase
    {
        private readonly ITodosClient _todosClient;

        public TodosGeneratedHttpClientController(ITodosClient todosClient)
        {
            _todosClient = todosClient;
        }

        [HttpGet]
        [Route("GetDataUsingGeneratedHttpClient")]
        public async Task<ActionResult> Index()
        {
            try
            {
                var todos = await _todosClient.GetTodos();
                return Ok(todos);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

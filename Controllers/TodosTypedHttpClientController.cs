using LearningHttpClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearningHttpClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //api/TodosTypedHttpClient/GetDataUsingTypedHttpClient
    public class TodosTypedHttpClientController : ControllerBase
    {
        private readonly ITodosService _todosService;

        public TodosTypedHttpClientController(ITodosService todosService)
        {
            _todosService = todosService;
        }

        [HttpGet]
        [Route("GetDataUsingTypedHttpClient")]
        public async Task<ActionResult> Index()
        {
            var todos = await _todosService.GetTodos();
            return Ok(todos);
        }
    }
}

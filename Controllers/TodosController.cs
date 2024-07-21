using LearningHttpClient.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LearningHttpClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //api/Todos
    public class TodosController : ControllerBase
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private static HttpClient _httpClient;

        // creating static constructor and creating http client instance
        // so that throught the application only one instance will be there
        static TodosController()
        {
            _httpClient = new HttpClient();
        }

        public TodosController()
        {
            _jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
        }

        [HttpGet]
        [Route("GetDataUsingNormalUsingHttpClient")]
        public async Task<ActionResult> Index()
        {
            var url = "https://jsonplaceholder.typicode.com/todos/";

            #region Problem for using HttpClient Directly
            //1. Creating new instance every time it will create socket issue, when no socket avaible to create it, it will faile
            //2. We have manage http client life time
            //3. When host ip address changes we have to handle it.
            // More Information https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
            #endregion

            #region Socket/connection Issue (HttpClient-Image1.png)
            // This will create new instance and tcp socket connection whenever we call this end point, and  we are not dispoing the http client and connection is established (active)
            // how to identity the ip address ping jsonplaceholder.typicode.com (url of the api), this application ip address will keep changing when we restart the application
            // we can see all the listed socket connections using netstat netstat -ano | findstr 172.67.167.151(Ipaddress)
            //var httpClient = new HttpClient();
            //var response = await httpClient.GetAsync(url);
            //response.EnsureSuccessStatusCode();

            //var todos = await response.Content.ReadFromJsonAsync<IEnumerable<Todo>>(_jsonSerializerOptions);
            //return Ok(todos);
            #endregion

            #region Socket/Connection Issue after closing/disposing the http client instance (HttpClient-Image2.png)
            // This will create new instance and tcp socket connection whenever we call this end point, and we are dispoing the http client, but connection is time_wait (not active)
            // how to identity the ip address ping jsonplaceholder.typicode.com (url of the api), this application ip address will keep changing when we restart the application
            // we can see all the listed socket connections using netstat netstat -ano | findstr 104.21.59.19(Ipaddress)
            //var httpClient = new HttpClient();
            //using (var client = new HttpClient())
            //{
            //    var response = await client.GetAsync(url);
            //    response.EnsureSuccessStatusCode();

            //    var todos = await response.Content.ReadFromJsonAsync<IEnumerable<Todo>>(_jsonSerializerOptions);
            //    return Ok(todos);
            //}
            #endregion

            #region SingleInstanceThrougoutApplicationUsingStaticConstructor (HttpClient-Image3.png)
            // here the application will have only instance how many times you call the api and connection will be active always
            // but problem here when host (jsonplaceholder.typicode.com) changes the IP address, that time request will get fail and we have to restart the application
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var todos = await response.Content.ReadFromJsonAsync<IEnumerable<Todo>>(_jsonSerializerOptions);
            return Ok(todos);
            #endregion
        }
    }
}

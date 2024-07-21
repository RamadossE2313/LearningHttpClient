using LearningHttpClient.Services;
using Refit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// basic usage of http client --> controller for this TodosBasicHttpClientController
builder.Services.AddHttpClient();

// named http client --> controller for this TodosNamedHttpClientController
builder.Services.AddHttpClient("todos", options =>
{
    options.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
});

// typed http client --> controller for this TodosTypedHttpClientController
builder.Services.AddHttpClient<ITodosService, TodosService>(options =>
{
    options.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
});

//IHttpClientFactory can be used in combination with third-party libraries such as Refit.
//Refit is a REST library for .NET. It converts REST APIs into live interfaces.
//Call AddRefitClient to generate a dynamic implementation of an interface,
//which uses HttpClient to make the external HTTP calls.

builder.Services.AddRefitClient<ITodosClient>()
    .ConfigureHttpClient(options =>
    {
        options.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
    });

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

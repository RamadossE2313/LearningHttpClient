using LearningHttpClient.Models.Common;
using LearningHttpClient.Services;
using LearningHttpClient.Services.HttpMessageHandlers;
using Refit;

// Shortcuts: select the code and press Ctrl+k+s ==> select the options and apply it

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration; // app
// Add services to the container.

// Configure method 
builder.Services.AddControllers();
builder.Services.AddTransient<DemoAuthHandler<Client1Configuration>>();
builder.Services.AddTransient<DemoAuthHandler<Client2Configuration>>();
builder.Services.Configure<Client1Configuration>(configuration.GetSection(Client1Configuration.ConfigurationKeyName));
builder.Services.Configure<Client2Configuration>(configuration.GetSection(Client2Configuration.ConfigurationKeyName));

#region configure method overload1
//builder.Services.Configure<Client1Configuration>(options =>
//{
//    options.ClientId = configuration["ClientId"];

//});

#endregion

#region configure method overload2
//builder.Services.Configure<Client1Configuration>(configuration, options =>
//{
//    /// <summary>
//    /// When false (the default), no exceptions are thrown when a configuration key is found for which the
//    /// provided model object does not have an appropriate property which matches the key's name.
//    /// When true, an <see cref="System.InvalidOperationException"/> is thrown with a description
//    /// of the missing properties.
//    /// </summary>
//    options.ErrorOnUnknownConfiguration = true;

//    /// <summary>
//    /// When false (the default), the binder will only attempt to set public properties.
//    /// If true, the binder will attempt to set all non read-only properties.
//    /// </summary>
//    options.BindNonPublicProperties = true;
//}); 
#endregion

//builder.Services.Configure<Client1Configuration>(Client1Configuration.ConfigurationKeyName, configuration);

//builder.Services.Configure<Client1Configuration>(Client1Configuration.ConfigurationKeyName, options =>
//{
//    options.ClientId = configuration["ClientId"];
//});

#region configure method overload5
//builder.Services.Configure<Client1Configuration>(Client1Configuration.ConfigurationKeyName, configuration, options =>
//{
//    // we will not get the Client1Configuration class properties
//    // only binding options will list it
//options.ErrorOnUnknownConfiguration = true;
//}); 
#endregion

// basic usage of http client --> controller for this TodosBasicHttpClientController
builder.Services.AddHttpClient();

// named http client --> controller for this TodosNamedHttpClientController
builder.Services.AddHttpClient("todos", options =>
{
    options.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
})
.AddHttpMessageHandler<DemoAuthHandler<Client2Configuration>>();

// typed http client --> controller for this TodosTypedHttpClientController
builder.Services.AddHttpClient<ITodosService, TodosService>(options =>
{
    options.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
})
.AddHttpMessageHandler<DemoAuthHandler<Client1Configuration>>();

//IHttpClientFactory can be used in combination with third-party libraries such as Refit.
//Refit is a REST library for .NET. It converts REST APIs into live interfaces.
//Call AddRefitClient to generate a dynamic implementation of an interface,
//which uses HttpClient to make the external HTTP calls.

//builder.Services.AddRefitClient<ITodosClient>()
//    .ConfigureHttpClient(options =>
//    {
//        options.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
//    }).AddHttpMessageHandler<DemoAuthHandler<Client2Configuration>>();


// based on the configuration toggle , we are using same message handler with different configuration
builder.Services.AddRefitClient<ITodosClient>(svc =>
{
    if (configuration["toggle"] == "SIT")
    {
        return new RefitSettings()
        {
            // HttpMessageHandlerFactory required func<HttpMessageHandler>,
            // we are retriveing services based on the toggle.
            HttpMessageHandlerFactory = () => svc.GetRequiredService<DemoAuthHandler<Client1Configuration>>()
        };
    }
    return new RefitSettings()
    {
        HttpMessageHandlerFactory = () => svc.GetRequiredService<DemoAuthHandler<Client2Configuration>>()
    };
})
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

namespace LearningHttpClient.Models.Common
{

    public class Client1Configuration : AuthoriztionBase 
    {
        public const string ConfigurationKeyName = nameof(Client1Configuration);
    }
    public class Client2Configuration : AuthoriztionBase 
    {
        public const string ConfigurationKeyName = nameof(Client2Configuration);
    }

    public abstract class AuthoriztionBase
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scopes { get; set; }
    }
}

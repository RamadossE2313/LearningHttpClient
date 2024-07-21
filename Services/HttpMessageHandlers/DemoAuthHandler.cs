using IdentityModel.Client;
using LearningHttpClient.Models.Common;
using Microsoft.Extensions.Options;

namespace LearningHttpClient.Services.HttpMessageHandlers
{
    public class DemoAuthHandler<T> : DelegatingHandler where T : AuthoriztionBase
    {
        private readonly HttpClient _httpClient;
        private readonly T _configuration;

        public DemoAuthHandler(HttpClient httpClient, IOptions<T> configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration.Value;
        }
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            DiscoveryDocumentResponse discoveryDocumentResponse = await _httpClient.GetDiscoveryDocumentAsync("");

            var tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discoveryDocumentResponse.TokenEndpoint,
                ClientId = _configuration.ClientId,
                ClientSecret = _configuration.ClientSecret,
                Scope = _configuration.Scopes,
            });

            request.Headers.Add("Authorization", $"bearer {tokenResponse.AccessToken}");

            return await base.SendAsync(request, cancellationToken);
        }
    }
}

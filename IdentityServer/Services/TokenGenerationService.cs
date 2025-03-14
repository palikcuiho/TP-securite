using System.Text.Json;
using Duende.IdentityServer.Models;
using IdentityServer.DTO;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace IdentityServer.Services
{
    public class TokenGenerationService(
        IHttpClientFactory httpClientFactory,
        IOptions<IdentityServerConfiguration> identityServerConfiguration
    ) : ITokenGenerationService
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient();
        private readonly string secret =
            Environment.GetEnvironmentVariable("SECRET")
            ?? throw new Exception("Secret missing from .env");
        private readonly IdentityServerConfiguration _identityServerConfiguration =
            identityServerConfiguration.Value;
        private static readonly JsonSerializerOptions _jsonWriteOptions = new()
        {
            WriteIndented = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Always,
        };

        public async Task<TokenResponse> GenerateToken(IdentityUser user, string password)
        {
            var tokenEndpoint = _identityServerConfiguration.TokenEndpoint;
            if (string.IsNullOrEmpty(tokenEndpoint))
            {
                throw new InvalidOperationException(
                    "TokenEndpoint is not configured properly in appsettings.json"
                );
            }

            var tokenRequest = new Dictionary<string, string>
            {
                { "client_id", _identityServerConfiguration.ClientId },
                { "client_secret", secret },
                { "grant_type", "password" },
                { "username", user.UserName ?? "" },
                { "email", user.Email ?? "" },
                { "password", password },
                { "scope", String.Join(",", _identityServerConfiguration.Scopes) },
            };

            var response = await _httpClient.PostAsync(
                tokenEndpoint,
                new FormUrlEncodedContent(tokenRequest)
            );
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"Failed to retrieve token: {responseContent}");
            }

            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(
                responseContent,
                _jsonWriteOptions
            );

            return tokenResponse
                ?? throw new InvalidOperationException($"Failed to deserialize token");
        }
    }
}

using System.Text.Json.Serialization;

namespace IdentityServer.DTO
{
    public class TokenResponse
    {
        public TokenResponse(string? accessToken, string? tokenType, int? expiresIn)
        {
            AccessToken = accessToken;
            TokenType = tokenType;
            ExpiresIn = expiresIn;
        }

        public TokenResponse(string error)
        {
            Error = error;
        }

        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string? TokenType { get; set; }

        [JsonPropertyName("expires_in")]
        public int? ExpiresIn { get; set; }

        [JsonPropertyName("error")]
        public string? Error { get; set; }
    }
}

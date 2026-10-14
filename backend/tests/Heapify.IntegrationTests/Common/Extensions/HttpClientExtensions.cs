using Heapify.Presentation.Auth.DTO;

namespace Heapify.IntegrationTests.Common.Extensions;

public static class HttpClientExtensions
{
    extension(HttpClient client)
    {
        public async Task<HttpClient> WithAntiforgeryAsync()
        {
            var response = await client.GetAsync("/api/v1/auth/antiforgery");
            response.EnsureSuccessStatusCode();

            var cookie = response.Headers.GetValues("Set-Cookie")
                .First(c => c.StartsWith("XSRF-TOKEN="));
            var token = Uri.UnescapeDataString(cookie.Split(';')[0]["XSRF-TOKEN=".Length..]);

            client.DefaultRequestHeaders.Remove("X-XSRF-TOKEN");
            client.DefaultRequestHeaders.Add("X-XSRF-TOKEN", token);
            return client;
        }

        public async Task<HttpClient> AuthenticateAsync(
            string username = "test", string email = "test@heapify.dev", string password = "Password123")
        {
            client = await client.WithAntiforgeryAsync();

            var registerRequest = new RegisterRequest { Username = username, Email = email, Password = password };
            var registerResponse = await client.PostAsJsonAsync("/api/v1/auth/register", registerRequest);
            registerResponse.EnsureSuccessStatusCode();

            var loginRequest = new LoginRequest { Email = email, Password = password };
            var loginResponse = await client.PostAsJsonAsync("/api/v1/auth/login", loginRequest);
            loginResponse.EnsureSuccessStatusCode();

            return await client.WithAntiforgeryAsync();
        }
    }
}
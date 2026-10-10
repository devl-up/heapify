using Heapify.Presentation.Auth.DTO;

namespace Heapify.IntegrationTests.Common.Extensions;

public static class HttpClientExtensions
{
    extension(HttpClient client)
    {
        private async Task<HttpClient> WithAntiforgeryAsync()
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

            var registerDto = new RegisterDto(username, email, password);
            var registerResponse = await client.PostAsJsonAsync("/api/v1/auth/register", registerDto);
            registerResponse.EnsureSuccessStatusCode();

            var loginDto = new LoginDto(email, password);
            var loginResponse = await client.PostAsJsonAsync("/api/v1/auth/login", loginDto);
            loginResponse.EnsureSuccessStatusCode();

            return await client.WithAntiforgeryAsync();
        }
    }
}
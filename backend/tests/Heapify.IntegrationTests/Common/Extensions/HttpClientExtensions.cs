using Heapify.IntegrationTests.Common.DTO;
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
            string email = TestUser.Email,
            string password = TestUser.Password)
        {
            client = await client.WithAntiforgeryAsync();

            var request = new LoginRequest { Email = email, Password = password };
            var response = await client.PostAsJsonAsync("/api/v1/auth/login", request);
            response.EnsureSuccessStatusCode();

            return await client.WithAntiforgeryAsync();
        }
    }
}
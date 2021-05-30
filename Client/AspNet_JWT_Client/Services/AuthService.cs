using AspNet_JWT_Client.Infrastructure;
using AspNet_JWT_Client.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AspNet_JWT_Client.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthService(HttpClient httpClient,
                   AuthenticationStateProvider authenticationStateProvider,
                   ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
        }

        public async Task<TokenResult> LoginAsync(string username, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:5001/token", new { Login = username, Password = password });

            var loginResult = JsonSerializer.Deserialize<TokenResult>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (response.IsSuccessStatusCode)
            {
                await _localStorage.SetAsync<string>("token", loginResult.AccessToken);
                ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginResult.AccessToken);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.AccessToken);

                return loginResult;
            }
            
            return loginResult;
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveAsync("token");
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}

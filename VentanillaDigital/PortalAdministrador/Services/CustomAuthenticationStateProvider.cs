using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using PortalAdministrador.Data.Account;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PortalAdministrador.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private ISessionStorageService _sessionStorageService;
        private ILocalStorageService _localStorageService;
        public CustomAuthenticationStateProvider(ISessionStorageService sessionStorageService
            , ILocalStorageService localStorageService)
        {
            _sessionStorageService = sessionStorageService;
            _localStorageService = localStorageService;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            AuthenticatedUser authenticatedUser = await _sessionStorageService.GetItemAsync<AuthenticatedUser>("authenticatedUser");
            ClaimsIdentity identity;
            if (authenticatedUser != null)
            {
                identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, authenticatedUser.Usuario),
                     new Claim(ClaimTypes.Role,authenticatedUser.Rol),
                    new Claim("Token", authenticatedUser.Token)
                }, "apiauth_type");
            }
            else
            {
                identity = new ClaimsIdentity();
            }

            var user = new ClaimsPrincipal(identity);
            return await Task.FromResult(new AuthenticationState(user));
        }
        public async Task<AuthenticatedUser> GetAuthenticatedUser()
        {
            return await _sessionStorageService.GetItemAsync<AuthenticatedUser>("authenticatedUser");
        }

        public async Task MarkUserAsAuthenticated(AuthenticatedUser authenticatedUser)
        {
            await _localStorageService.SetItem("token", authenticatedUser.Token);
            await _sessionStorageService.SetItemAsync("authenticatedUser", authenticatedUser);

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name,authenticatedUser.Usuario),
                new Claim(ClaimTypes.Role,authenticatedUser.Rol),
                new Claim("Token", authenticatedUser.Token)
            }, "apiauth_type");

            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));

        }
        public async Task MarkUserAsLoggedOut()
        {
            await _sessionStorageService.RemoveItemAsync("authenticatedUser");
            await _localStorageService.RemoveItem("token");
            var identity = new ClaimsIdentity();

            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    }
}

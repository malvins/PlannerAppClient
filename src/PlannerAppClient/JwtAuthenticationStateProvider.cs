using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PlannerAppClient
{
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _storage;
        public JwtAuthenticationStateProvider(ILocalStorageService storage)
        {
            _storage = storage;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if(await _storage.ContainKeyAsync("access_token"))
            {
                /*
                 * user is logged in
                 */
                var tokenStr = await _storage.GetItemAsStringAsync("access_token");
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.ReadJwtToken(tokenStr);
                var identity = new ClaimsIdentity(token.Claims, "Bearer");
                var user = new ClaimsPrincipal(identity);
                var authState = new AuthenticationState(user);
                NotifyAuthenticationStateChanged(Task.FromResult(authState));
                return authState;
            }
            return new AuthenticationState(new ClaimsPrincipal());
            /*
             * empty claim means no identity and user not logged in
             */
        }
    }
}

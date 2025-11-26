using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Orders.Frontend.Helpers;
using Orders.Frontend.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace Orders.Frontend.AuthenticationProviders
{
    public class AuthenticationProviderJWT : AuthenticationStateProvider, ILoginService //Hereda de la clase abstracta AuthenticacionStateProvider y de ILoginService
    {
        private readonly IJSRuntime _jSRuntime; //Interoperabilidad (método de extension: añado más métodos)
        private readonly HttpClient _httpClient; // Necesito el Token para las peticiones.
        private readonly string _tokenKey;
        private readonly AuthenticationState _anonimous;

        public AuthenticationProviderJWT(IJSRuntime jSRuntime, HttpClient httpClient)
        {
            _jSRuntime = jSRuntime;
            _httpClient = httpClient;
            _tokenKey = "TOKEN_KEY"; //Como lo voy a almacenar el LocalStorage
            _anonimous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));//Usuario anónimo
        }

        //METODOS PÚBLICOS
        //-----------------
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await _jSRuntime.GetLocalStorage(_tokenKey);
                if (token is null)
                {
                    return _anonimous;
                }
                return BuildAuthenticationState(token.ToString()!);
            }
            catch (InvalidOperationException)
            {
                return _anonimous;
            }
        }

        //Metodo login, paso usuario y contraseña y me devuelve un Token. Con ese Token hago lo siguiente:
        public async Task LoginAsync(string token)
        {
            await _jSRuntime.SetLocalStorage(_tokenKey, token); //Lo guardo en el LocalStorage
            var authState = BuildAuthenticationState(token); //Creame el AuthenticationStorage con ese Token.
            NotifyAuthenticationStateChanged(Task.FromResult(authState));//Devuelve el usuario en el AuthentificationStage
        }
        
        //Método para salir. Quita el StorageAccount.
        public async Task LogoutAsync()
        {
            await _jSRuntime.RemoveLocalStorage(_tokenKey);
            _httpClient.DefaultRequestHeaders.Authorization = null;
            NotifyAuthenticationStateChanged(Task.FromResult(_anonimous));//Devuelve un usuario anonimo en el AuthentificationStage
        }

        //METODOS PRIBADOS
        //-----------------
        private AuthenticationState BuildAuthenticationState(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            var claims = ParseClaimsFromJWT(token);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")));
        }

        private IEnumerable<Claim> ParseClaimsFromJWT(string token)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var unserializedToken = jwtSecurityTokenHandler.ReadJwtToken(token);
            return unserializedToken.Claims;
        }

       
    }

}

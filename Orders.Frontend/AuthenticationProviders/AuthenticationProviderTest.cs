using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Orders.Frontend.AuthenticationProviders
{
    public class AuthenticationProviderTest : AuthenticationStateProvider //Hereda de la clase abstracta AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            await Task.Delay(1000);
            var anonimous = new ClaimsIdentity(); 

            //Creamos el usuario
            var user = new ClaimsIdentity(authenticationType: "test");

            //Creamos el usuario administrador
            var admin = new ClaimsIdentity(
        [
            new("FirstName", "Juan"),
            new("LastName", "Zulu"),
            new(ClaimTypes.Name, "zulu@yopmail.com"), //Persona que está autenticada.
            new(ClaimTypes.Role, "Admin") //Me devuelve un admin
        ],
            authenticationType: "test");

            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(admin)));//Me dice que el usuario es anónimo.
        }
    }
}



using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Orders.Frontend.Components.Pages.Auth;
using Orders.Frontend.Repositories;
using Orders.Frontend.Services;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Enums;

namespace Orders.Frontend.Components.Pages.Shared
{
    public partial class AuthLinks
    {
        private string? photoUser;

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private IDialogService DialogService { get; set; } = null!;
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!; //Para saber si está o no logueado

        protected override async Task OnParametersSetAsync()
        {
            var authenticationState = await AuthenticationStateTask; //Cuando cargue, verifica el AuthenticationStage
            var claims = authenticationState.User.Claims.ToList(); 
            var photoClaim = claims.FirstOrDefault(x => x.Type == "Photo");//Verifica si el usuario tiene foto
            var nameClaim = claims.FirstOrDefault(x => x.Type == "UserName");//Verifica si el usuario tiene nombre (para saludar)
            if (photoClaim is not null)
            {
                photoUser = photoClaim.Value;
            }
        }

        //Si pulsa editar le mandamos a la función de Editar usuario
        private void EditAction()
        {
            NavigationManager.NavigateTo("/EditUser");
        }

        //Muestra el login
        private void ShowModalLogIn()
        {
            var closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true };
            DialogService.ShowAsync<Login>("Inicio de Sesion", closeOnEscapeKey);
        }

        //Muestra la pantalla de cerrar Sesión
        private void ShowModalLogOut()
        {
            var closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true };
            DialogService.ShowAsync<Logout>("Cerrar Sesion", closeOnEscapeKey);
        }

        //Muestra la pantalla de Registro de Usuario
        private void ShowModalRegister()
        {
            var closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true };
            DialogService.ShowAsync<Register>("Registar Usuario", closeOnEscapeKey);
        }
    }


}
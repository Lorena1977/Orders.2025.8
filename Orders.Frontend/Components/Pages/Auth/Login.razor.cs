using Microsoft.AspNetCore.Components;
using MudBlazor;
using Orders.Frontend.Repositories;
using Orders.Frontend.Services;
using Orders.Shared.DTOs;


namespace Orders.Frontend.Components.Pages.Auth
{
    public partial class Login
    {
        private LoginDTO loginDTO = new(); //mail y contraseña.
        private bool wasClose;

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private IDialogService DialogService { get; set; } = null!;
        [Inject] private ISnackbar Snackbar { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private ILoginService LoginService { get; set; } = null!;
        
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
        
        //Cuando el usuario pulse sobre confirmación.
        private void ShowModalResendConfirmationEmail()
        {
            var closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true, CloseButton = true, MaxWidth = MaxWidth.ExtraLarge };
            DialogService.ShowAsync<ResendConfirmationEmailToken>("Reenvio de correo", closeOnEscapeKey);
        }

        //Cuando el usaario pulse Restablecer contraseña.
        private void ShowModalRecoverPassword()
        {
            var closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true, CloseButton = true, MaxWidth = MaxWidth.ExtraLarge };
            DialogService.ShowAsync<RecoverPassword>("Rec. contraseña", closeOnEscapeKey);
        }

        //Cuando el usaario pulse Cerrar
        private void CloseModal()
        {
            wasClose = true;
            MudDialog.Cancel();
        }

        //Cuando el usaario pulse Login
        private async Task LoginAsync()
        {
            if (wasClose)
            {
                NavigationManager.NavigateTo("/");
                return;
            }

            var responseHttp = await Repository.PostAsync<LoginDTO, TokenDTO>("/api/accounts/Login", loginDTO); //Con el usuario y contraseña devuelve un Token
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                Snackbar.Add(message!, Severity.Error);
                return;
            }

            await LoginService.LoginAsync(responseHttp.Response!.Token);//Llamamos al LoginService con el Token recuperado.
            NavigationManager.NavigateTo("/");
        }
    }


}
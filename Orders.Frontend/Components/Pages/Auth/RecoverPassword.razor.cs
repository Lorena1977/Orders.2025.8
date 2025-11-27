using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Components.Snackbar.InternalComponents;
using Orders.Frontend.Repositories;
using Orders.Shared.DTOs;

namespace Orders.Frontend.Components.Pages.Auth
{
    public partial class RecoverPassword
    {
        private EmailDTO emailDTO = new();
        private bool loading;

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private ISnackbar SnackbarService { get; set; } = null!;
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        private async Task SendRecoverPasswordEmailTokenAsync()
        {
            loading = true;
            var responseHttp = await Repository.PostAsync("/api/accounts/RecoverPassword", emailDTO);
            loading = false;
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                //await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                //loading = false;
               
                SnackbarService.Add(message, Severity.Error, config => { });
                return;
            }

            MudDialog.Cancel();
            NavigationManager.NavigateTo("/");
            SnackbarService.Add("Se te ha enviado un correo electrónico con las instrucciones para recuperar su contraseña.", Severity.Success);

            //loading = false;
            //await SweetAlertService.FireAsync("Confirmación", "Se te ha enviado un correo electrónico con las instrucciones para recuperar su contraseña.", SweetAlertIcon.Info);
            //NavigationManager.NavigateTo("/");
        }
    }
}


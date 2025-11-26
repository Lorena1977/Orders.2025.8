using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Orders.Frontend.Repositories;
using Orders.Frontend.Services;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Enums;

namespace Orders.Frontend.Components.Pages.Auth
{
    public partial class Register
    {
        private UserDTO userDTO = new(); //Todos los campos de usuario + contraseña + confirmación de contraseña
        private List<Country>? countries; //Lista de paises
        private List<State>? states; //Lista de estados
        private List<City>? cities; //Lista de ciudades.
        private bool loading; 
        private string? imageUrl;//Atributo que es la imagen del usuario cuando se registra.
        private string? titleLabel;

        private Country selectedCountry = new();
        private State selectedState = new();
        private City selectedCity = new();

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private ILoginService LoginService { get; set; } = null!; //Si me registro automáticamente lo loggeo. Eso lo vamos a quitar.
        [Inject] private IDialogService DialogService { get; set; } = null!;
        [Inject] private ISnackbar Snackbar { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Parameter, SupplyParameterFromQuery] public bool IsAdmin { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadCountriesAsync(); //Carga los Paises (Va al API y carga el combo)
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            titleLabel = IsAdmin ? "Registro de Administrador" : "Registro de Usuario";
        }

        //Método que me devuelve la imagen en base64 (cuando se registra el usuario).
        private void ImageSelected(string imageBase64)
        {
            userDTO.Photo = imageBase64;
            imageUrl = null;
        }

        //Método que devuelve los Paises
        private async Task LoadCountriesAsync()
        {
            var responseHttp = await Repository.GetAsync<List<Country>>("/api/countries/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                Snackbar.Add(message!, Severity.Error);
                return;
            }
            countries = responseHttp.Response;
        }

        //Método que devuelve lo Estados asociados al pais seleccionado
        private async Task LoadStatesAsyn(int countryId)
        {
            var responseHttp = await Repository.GetAsync<List<State>>($"/api/states/combo/{countryId}");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                Snackbar.Add(message!, Severity.Error);
                return;
            }
            states = responseHttp.Response;
        }

        //Método que devuelve las ciuedades asociadas al estado seleccionado.
        private async Task LoadCitiesAsyn(int stateId)
        {
            var responseHttp = await Repository.GetAsync<List<City>>($"/api/cities/combo/{stateId}");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                Snackbar.Add(message!, Severity.Error);
                return;
            }
            cities = responseHttp.Response;
        }

        //Método que me limpia la ciudad y el estado cuando cambio el pais.
        private async Task CountryChangedAsync(Country country)
        {
            selectedCountry = country;
            selectedState = new State();
            selectedCity = new City();
            states = null;
            cities = null;
            await LoadStatesAsyn(country.Id);
        }

        //Método que limpia la ciudad cuando cambio de estado.
        private async Task StateChangedAsync(State state)
        {
            selectedState = state;
            selectedCity = new City();
            cities = null;
            await LoadCitiesAsyn(state.Id);
        }

        //Método que permite cambiar una ciudad.
        private void CityChanged(City city)
        {
            selectedCity = city;
            userDTO.CityId = city.Id;
        }

        //Método que no me muestra todos los paises sino que muestra unos pocos y luego pueda buscar.
        private async Task<IEnumerable<Country>> SearchCountries(string searchText, CancellationToken token)
        {
            await Task.Delay(5);
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return countries!;
            }

            return countries!
                .Where(c => c.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        private async Task<IEnumerable<State>> SearchStates(string searchText, CancellationToken token)
        {
            await Task.Delay(5);
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return states!;
            }

            return states!
                .Where(c => c.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        private async Task<IEnumerable<City>> SearchCity(string searchText, CancellationToken token)
        {
            await Task.Delay(5);
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return cities!;
            }

            return cities!
                .Where(c => c.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        //Si el usuario no se registró se sale.
        private void ReturnAction()
        {
            NavigationManager.NavigateTo("/");
        }

        //Metodo que comprueba que se hayan seleccionado todos los campos.
        private void InvalidForm()
        {
            Snackbar.Add("Por favor llena todos los campos del formulario.", Severity.Warning);
        }

        //Método que permite crear un usuario. Si pudo, lo registra y manda el correo.
        private async Task CreateUserAsync()
        {
            loading = true;
            userDTO.UserName = userDTO.Email;
            userDTO.UserType = UserType.User;
            var responseHttp = await Repository.PostAsync<UserDTO>("/api/accounts/CreateUser", userDTO);
            loading = false;
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            //En vez de loggearse, mandamos un mensaje de confirmación.

            await SweetAlertService.FireAsync("Confirmación", "Su cuenta ha sido creada con éxito. Se te ha enviado un correo electrónico con las instrucciones para activar tu usuario.", SweetAlertIcon.Info);
            NavigationManager.NavigateTo("/");

            //if (userDTO.Email is null || userDTO.PhoneNumber is null)
            //{
            //    InvalidForm();
            //    return;
            //}

            //userDTO.UserType = UserType.User;
            //userDTO.UserName = userDTO.Email;

            //if (IsAdmin)
            //{
            //    userDTO.UserType = UserType.Admin;
            //}

            //loading = true;
            //var responseHttp = await Repository.PostAsync<UserDTO, TokenDTO>("/api/accounts/CreateUser", userDTO);
            //loading = false;
            //if (responseHttp.Error)
            //{
            //    var message = await responseHttp.GetErrorMessageAsync();
            //    Snackbar.Add(message!, Severity.Error);
            //    return;
            //}

            //await LoginService.LoginAsync(responseHttp.Response!.Token);
            //NavigationManager.NavigateTo("/");
        }
    }


}
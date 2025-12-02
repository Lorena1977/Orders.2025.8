using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Orders.Frontend.Components.Pages.Countries;
using Orders.Frontend.Repositories;
using Orders.Shared.Entities;

namespace Orders.Frontend.Components.Pages.Products
{
    [Authorize(Roles = "Admin")]
    public partial class ProductsIndex
    {
        private int currentPage = 1;
        private int totalPages;
        public List<Product>? Products { get; set; }

        private bool loading; //Cosecha propia
        private MudTable<Product> table = new();//Cosecha propia
        private readonly int[] pageSizeOptions = { 10, 25, 50, int.MaxValue };//Cosecha propia
        private const string baseUrl = "api/products";//Cosecha propia
        private string infoFormat = "{first_item}-{last_item} => {all_items}";//Cosecha propia
        


        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private IDialogService DialogService { get; set; } = null!; //Cosecha propia
        [Inject] private ISnackbar Snackbar { get; set; } = null!; //Cosecha propia


        [Parameter, SupplyParameterFromQuery] public string Page { get; set; } = string.Empty;
        [Parameter, SupplyParameterFromQuery] public string Filter { get; set; } = string.Empty;
        [Parameter, SupplyParameterFromQuery] public int RecordsNumber { get; set; } = 10;
        
        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }


        private async Task SelectedRecordsNumberAsync(int recordsnumber)
        {
            RecordsNumber = recordsnumber;
            int page = 1;
            await LoadAsync(page); //Carga los productos del Api
            await SelectedPageAsync(page);
        }

        private async Task FilterCallBack(string filter)
        {
            Filter = filter;
            await ApplyFilterAsync();
            StateHasChanged();
        }

        private async Task SelectedPageAsync(int page)
        {
            currentPage = page;
            await LoadAsync(page);
        }

        private async Task LoadAsync(int page = 1)
        {
            if (!string.IsNullOrWhiteSpace(Page))
            {
                page = Convert.ToInt32(Page);
            }

            var ok = await LoadListAsync(page);
            if (ok)
            {
                await LoadPagesAsync();
            }
        }

        private void ValidateRecordsNumber(int recordsnumber)
        {
            if (recordsnumber == 0)
            {
                RecordsNumber = 10;
            }
        }

        //Carga la lista de productos del API que acabamos de crear.
        private async Task<bool> LoadListAsync(int page)
        {
            ValidateRecordsNumber(RecordsNumber);
            var url = $"api/products?page={page}&recordsnumber={RecordsNumber}";
            if (!string.IsNullOrEmpty(Filter))
            {
                url += $"&filter={Filter}";
            }

            var response = await Repository.GetAsync<List<Product>>(url);
            if (response.Error)
            {
                var message = await response.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);              
                   return false;
                }
                Products = response.Response;
                return true;
            }

        //Carga la la variable de totalPages.
        private async Task LoadPagesAsync()
        {
            ValidateRecordsNumber(RecordsNumber);
            var url = $"api/products/totalPages?recordsnumber={RecordsNumber}";
            if (!string.IsNullOrEmpty(Filter))
            {
                url += $"&filter={Filter}";
            }

            var response = await Repository.GetAsync<int>(url);
            if (response.Error)
            {
                var message = await response.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            totalPages = response.Response;
        }

        //Método para borrar el producto.
        private async Task Delete(int productId)
        {
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmación",
                Text = "¿Esta seguro que quieres borrar el registro?",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true
            });
            var confirm = string.IsNullOrEmpty(result.Value);

            if (confirm)
            {
                return;
            }

            var responseHttp = await Repository.DeleteAsync($"api/products/{productId}");

            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo("/");
                    return;
                }

                var mensajeError = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", mensajeError, SweetAlertIcon.Error);
                return;
            }

            await LoadAsync(1);
        }

        //Método para aplicar el filtro.
        private async Task ApplyFilterAsync()
        {
            int page = 1;
            await LoadAsync(page);
            await SelectedPageAsync(page);
        }

        //------------------------------------------------------------------- INICIO
       
        
        //------------------------------------------------------------------- FIN





    }
}
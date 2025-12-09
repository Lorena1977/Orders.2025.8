using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor.Services;
using Orders.Frontend.Components;
using Orders.Frontend.AuthenticationProviders;
using Orders.Frontend.Repositories;
using Orders.Frontend.Services;


var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

//Configuramos la dirección por la que sale nuestro Backend
//AddScoped: cada vez que inyecto me genera un objeto nuevo cada vez que yo lo inyecte.
//AddTrasient: cada vez que inyecto lo está llamando por cada petición Http.
//AddSingleton: Se mantiene una única instancia del objeto por todo el ciclo de vida de la aplicacion.(están en memoria)
builder.Services.AddSingleton(_ => new HttpClient { BaseAddress = new Uri("https://localhost:7111") });
//builder.Services.AddSingleton(_ => new HttpClient { BaseAddress = new Uri("http://localhost:5219") });

// Necesita que le autoricen las cosas, y vamos a usar el AuthenticatioProviderTest creado.
builder.Services.AddAuthorizationCore();
//builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationProviderTest>(); //Se borra porque incluimos el AuthentificationProviderJWT

// configuramos la inyección del SweetAlert 
builder.Services.AddSweetAlert2();

builder.Services.AddScoped<IRepository, Repository>();

//Inyectamos nuestro nuevo proveedor de Autentificación. Para que tome el LoginService.
builder.Services.AddScoped<AuthenticationProviderJWT>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationProviderJWT>(x => x.GetRequiredService<AuthenticationProviderJWT>());
builder.Services.AddScoped<ILoginService, AuthenticationProviderJWT>(x => x.GetRequiredService<AuthenticationProviderJWT>());

//  Activar errores detallados de circuitos Blazor Server
builder.Services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });
builder.Services.AddMudServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts
    
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

// Middlewares
//app.UseStaticFiles();
//app.UseRouting();
//app.MapFallbackToFile("index.html"); // importante para WASM


app.Run();

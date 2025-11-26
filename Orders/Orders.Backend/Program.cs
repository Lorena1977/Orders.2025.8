using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Orders.Backend.Data;
using Orders.Backend.Helpers;
using Orders.Backend.Repositories.Implementations;
using Orders.Backend.Repositories.Interfaces;
using Orders.Backend.Services;
using Orders.Backend.UnitsOfWork.Implementations;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.Entities;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();

//228. Habilitamos Tokens en el Swagger. 
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Orders Backend", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. <br /> <br />
                      Enter 'Bearer' [space] and then your token in the text input below.<br /> <br />
                      Example: 'Bearer 12345abcdef'<br /> <br />",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,
            },
            new List<string>()
          }
        });
});

//6. Inyecto la conexión con el SqlServer
builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer("name=LocalConnection"));

//22. Inyectamos los servicios 
builder.Services.AddScoped(typeof(IGenericUnitOfWork<>), typeof(GenericUnitOfWork<>));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

//44. Agregamos las nuevas inyecciones de countries
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<ICountriesUnitOfWork, CountriesUnitOfWork>();

//52. Agregamos las nuevas inyecciones de estados
builder.Services.AddScoped<IStatesRepository, StatesRepository>();
builder.Services.AddScoped<IStatesUnitOfWork, StatesUnitOfWork>();

//81. Agregamos las nuevas inyecciones de las ciudades
builder.Services.AddScoped<ICitiesRepository, CitiesRepository>();
builder.Services.AddScoped<ICitiesUnitOfWork, CitiesUnitOfWork>();

//129. Agregamos las inyecciones de categorías
builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();
builder.Services.AddScoped<ICategoriesUnitOfWork, CategoriesUnitOfWork>();

//188. Agregamos la inyeccion de los usuarios
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IUsersUnitOfWork, UsersUnitOfWork>();

//259. Agregamos la inyección que me permite guardar/borrar imagenes en el BlobStorage.
builder.Services.AddScoped<IFileStorage, FileStorage>();

//24. Inyectamos el SeedDb
builder.Services.AddTransient<SeedDb>();
builder.Services.AddScoped<IApiService, ApiService>();

//300. Configuramos la inyección del servicio.
builder.Services.AddScoped<IMailHelper, MailHelper>();

//348 Agreamos la inyección de los productos.
builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
builder.Services.AddScoped<IProductsUnitOfWork, ProductsUnitOfWork>();

//188. Añadimos además cuales son las condiciones del Password que tiene que introducir usuario.
builder.Services.AddIdentity<User, IdentityRole>(x =>
{
    x.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider; //Necesitamos que el usuario tenga un mail confirmado
    x.SignIn.RequireConfirmedEmail = true;

    x.User.RequireUniqueEmail = true;//El usuario requiere un único email
    x.Password.RequireDigit = false; //No requiere digitos
    x.Password.RequiredUniqueChars = 0; //0 caracteres especiales
    x.Password.RequireLowercase = false; //No requiere minúsculas
    x.Password.RequireNonAlphanumeric = false; 
    x.Password.RequireUppercase = false; //No requiere mayusculas
    
    //Esto le pone seguridad al sistema. Habilitamos el bloqueo
    x.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); //Cuando un usuario se equivoque n-veces (3 veces) ingresando el password
                                                                // lo bloqueamos 5 minutos lo bloqueamos 5 minutos
    x.Lockout.MaxFailedAccessAttempts = 3; //Número de Intentos.
    x.Lockout.AllowedForNewUsers = true;
})
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

//Agregamos al backend autentificación
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(x => x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwtKey"]!)),
        ClockSkew = TimeSpan.Zero
    });

var app = builder.Build();


//27. Cada vez que se ejecuta el programa, llamamos al SeedDB
SeedData(app);
void SeedData(WebApplication app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory!.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<SeedDb>();
        service!.SeedAsync().Wait();
    }
}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    // Esto permite que se muestren las excepciones completas en el navegador durante desarrollo
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles(); //para servir archivos desde wwwroot

app.MapControllers();

//habilita el consumo desde el Frontend
app.UseCors(x => x
    .AllowAnyMethod()//Cualquiera puede consumir esos metodos (post, put, get)
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());


app.Run();

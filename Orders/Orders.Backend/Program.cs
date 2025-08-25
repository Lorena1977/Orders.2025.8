using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Vamos a cerar una base de datos que va a estar en el servidor 
builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("DockerConnection")));

//Para permitir solicitudes desde cualquier sitio
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();   
}
app.UseCors();         // Activar CORS
app.UseStaticFiles();  // Servir archivos estáticos

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Share.Entities;

namespace Orders.Backend.Controllers;

[ApiController] //Le decimos que es un API Controller
[Route("api/[controller]")] //Lo reemplaza por el nombre del controlador

public class CountriesController: ControllerBase //Hereda de la clase ControllerBase
{
    //Creamos un constructor
    //Queremos que la inyección funcione durante toda la ejecución de la clase

    private readonly DataContext _context;

    public CountriesController(DataContext context) //Inyección clasica. Cuando yo llame a esta clase
        //le tengo que pasar un DataContext, va al fichero Program que es quien sabe como funciona el DataContext
        //donde indica que es una instancia de SQLSERVER que utiliza el DockerConnection.
        //De esta manera creamos la conexión con la base de datos.
    {
        _context = context; //_context es mi base de datos
    }

    [HttpGet] //Se utiliza para obtener los Paises
    //Creamos el método asyncrono porque usa mejor los recuros de la máquina
    public async Task<IActionResult> GetAsync() 
    {
        //Metodo que devuelve una lista de la coleción countries del ontexto
        return Ok(await _context.Countries.ToListAsync());      
    }


    [HttpPost] //Se utiliza para crear.
    //Creamos el método asyncrono porque usa mejor los recuros de la máquina
    public async Task<IActionResult> PostAsync(Country country) //Cada vez que cree un pais me van a mandar un modelo country
    {
        //Agregamos este campo a la base de datos
        _context.Countries.Add(country);
        await _context.SaveChangesAsync(); //Hace el commit de la transacción utiliza mejor el procesador de la máquina 
        return Ok(country);
    }

}

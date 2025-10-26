using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;

namespace Orders.Backend.Controllers
{
    [ApiController] //Le indicamos que es un API controller
    [Route("api/[controller]")]//Lo ruteamos


    //Como hemos creado genéricos, actualizamos este controlador por el genérico.
    //----------------------------------------------------------------------------
    public class CountriesController : GenericController<Country>
    {
        private readonly ICountriesUnitOfWork _countriesUnitOfWork;

        public CountriesController(IGenericUnitOfWork<Country> unit, ICountriesUnitOfWork countriesUnitOfWork) : base(unit)
        {
            _countriesUnitOfWork = countriesUnitOfWork;
        }
        [HttpGet]
        public override async Task<IActionResult> GetAsync()
        {
            var response = await _countriesUnitOfWork.GetAsync();
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var response = await _countriesUnitOfWork.GetAsync(id);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }

        //Añade el Get para que muestre los paises paginados.
        [HttpGet("paginated")]
        public override async Task<IActionResult> GetAsync(PaginationDTO pagination)
        {
            var response = await _countriesUnitOfWork.GetAsync(pagination);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("totalRecords")]
        public override async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _countriesUnitOfWork.GetTotalRecordsAsync(pagination);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }


    }

    ////Creamos el constructor de la clase y le inyectamos al constructor la bas de datos
    //public class CountriesController : ControllerBase
    //{
    //    private readonly DataContext _context;

    //    public CountriesController(DataContext context) //Inyección clásica. Cuando yo llamo a esta clase
    //        //la tenqo que pasar un DataContext, va al fichero Programa que es quien sabe como 
    //        //funciona el DataContext donde indica que es una instancia de SQLSERVER que utiliza el LocalConnection
    //    {
    //        _context = context;
    //    }

    //    //-----------------------------------------
    //    //Devuelve los Paises de la base de datos.
    //    [HttpGet]
    //    public async Task<IActionResult> GetAsync()
    //    {
    //        //Devuelve una lista de la colección Countries del contexto.
    //        return Ok(await _context.Countries.ToListAsync());
    //    }

    //    //--------------------------------------------------------
    //    //Devuelve un pais a través de su Id de la base de datos
    //    [HttpGet("{id}")]
    //    public async Task<IActionResult> GetAsync(int id)
    //    {
    //        var country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == id);
    //        if (country == null)
    //        {
    //            return NotFound();
    //        }

    //        return Ok(country);
    //    }

    //    //-----------------------------------------
    //    // Crea un registro en la base de datos
    //    [HttpPost]
    //    //Creamos el método asíncrono porque usa mejor los recursos de la máquina
    //    public async Task<IActionResult> PostAsync(Country country)//Cada vez que cree un pais va a mandar un modelo Country
    //    {
    //        //Agregamos este campo a la base de datos.
    //        _context.Add(country);
    //        await _context.SaveChangesAsync();//Hace el commit de la transacción. Utiliza mejor el procesador de la máquina
    //        return Ok(country);
    //    }

    //    //-----------------------------------------
    //    //Borra un registro a través del Id
    //    [HttpDelete("{id}")]
    //    public async Task<IActionResult> DeleteAsync(int id)
    //    {
    //        var country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == id);
    //        if (country == null)
    //        {
    //            return NotFound();
    //        }

    //        _context.Remove(country);
    //        await _context.SaveChangesAsync();
    //        return NoContent();
    //    }

    //    //-----------------------------------------
    //    //Actualiza los registros de una tabla
    //    [HttpPut]
    //    public async Task<IActionResult> PutAsync(Country country)
    //    {
    //        _context.Update(country);
    //        await _context.SaveChangesAsync();
    //        return Ok(country);
    //    }
    //}



}

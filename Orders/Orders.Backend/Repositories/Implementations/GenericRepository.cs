using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Backend.Helpers;
using Orders.Backend.Repositories.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Responses;

namespace Orders.Backend.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class //Implementa a la interfaz IGenericRepository
    {
        //1. Atributos privados.
        //---------------------
        private readonly DataContext _context; //Inyección del context. (para que sirva para todo el ciclo de vida y no solo para el constructor).
        private readonly DbSet<T> _entity; //Inyeccion de las entidades genéricas.

        //2. Constructor de la clase
        //---------------------------
        public GenericRepository(DataContext context)
        {
            _context = context;
            _entity = context.Set<T>();
        }

        //3. Métodos públicos
        //-------------------
        //3.1. Método de Insercción
        //--------------------------
        public virtual async Task<ActionResponse<T>> AddAsync(T entity)
        {
            _context.Add(entity); //Adiciona la entidad a la base de datos.
            try //Validamos los errores antes de guardar la base de datos.
            {
                await _context.SaveChangesAsync(); //Graba los cambios en la base de datos
                return new ActionResponse<T> //Si va bien, devuelve un ActionResponse (clase)
                {
                    WasSuccess = true,
                    Result = entity
                };
            }
            catch (DbUpdateException)
            {
                return DbUpdateExceptionActionResponse();
            }
            //Añado varios Catch que se van evaluando en orden
            catch (Exception exception)
            {
                return ExceptionActionResponse(exception);//Método que me devuelve error en el update.
            }
        }

        //3.2. Método de Borrado
        //--------------------------
        public virtual async Task<ActionResponse<T>> DeleteAsync(int id)
        {
            //Lo buscamos. Creamos una propiedad row y le diga que la busque en la entidad
            var row = await _entity.FindAsync(id);
            if (row == null)
            {
                return new ActionResponse<T>
                {
                    WasSuccess = false,
                    Message = "Registro no encontrado"
                };
            }

            try
            {               
                _entity.Remove(row);//si la encuentra la borramos de la base de datos
                await _context.SaveChangesAsync(); // Salvamos los datos en la base de datos
                return new ActionResponse<T>
                {
                    WasSuccess = true,
                };
            }
            catch
            {
                return new ActionResponse<T> //Tiene registros duplicados
                {
                    WasSuccess = false,
                    Message = "No se puede borrar, porque tiene registros relacionados"
                };
            }
        }

        //3.3. Método de obtencion por Id
        //--------------------------------
        public virtual async Task<ActionResponse<T>> GetAsync(int id)
        {
            //lo buscamos. Creamos una propiedad row y le digo que la busque en la entidad
            var row = await _entity.FindAsync(id);
            if (row != null)
            {
                return new ActionResponse<T>
                {
                    WasSuccess = true,
                    Result = row
                };
            }
            return new ActionResponse<T>
            {
                WasSuccess = false,
                Message = "Registro no encontrado"
            };
        }

        //3.4. Método de obtencion de todos los registros
        //------------------------------------------------
        public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsync()
        {
            return new ActionResponse<IEnumerable<T>>
            {
                WasSuccess = true, //devuelve una lista vacía o no
                Result = await _entity.ToListAsync() // Me devuelve genericamente la lista de las entidades
            };
        }

        //3.5. Método de Actualización
        //----------------------------
        public virtual async Task<ActionResponse<T>> UpdateAsync(T entity)
        {
            try
            {
                _context.Update(entity); //Adiciona una entidad (pais, country....)
                await _context.SaveChangesAsync();//Graba los cambios en la base de datos
                return new ActionResponse<T> //Si va bien, devuelve un ActionRepesponse (clase)
                {
                    WasSuccess = true,
                    Result = entity
                };
            }

            //Añado varios catch que se van evaluando en orden
            catch (DbUpdateException)
            {
                return DbUpdateExceptionActionResponse(); //Método que me devuelve error en el update
            }
            catch (Exception exception)
            {
                return ExceptionActionResponse(exception);
            }
        }

        //3.6. Método de obtención paginada
        //---------------------------------
        public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _entity.AsQueryable();

            return new ActionResponse<IEnumerable<T>> //Devuelvame el restultado paginado
            {
                WasSuccess = true,
                Result = await queryable
                    .Paginate(pagination)
                    .ToListAsync()
            };
        }

        //3.1. Método de obtención del total de registros
        //--------------------------------------------------
        public virtual async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
        {
            var queryable = _entity.AsQueryable();
            double count = await queryable.CountAsync();
            return new ActionResponse<int>
            {
                WasSuccess = true,
                Result = (int)count
            };
        }

        //4. Métodos Privados
        //------------------------
        //4.1. Método con otra excepción diferente
        //---------------------------------------
        private ActionResponse<T> ExceptionActionResponse(Exception exception)
        {
            return new ActionResponse<T>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }

      // 4.2.Método para controlar errores en el Update de la base de datos
      //-------------------------------------------------------------------
        private ActionResponse<T> DbUpdateExceptionActionResponse()
        {
            return new ActionResponse<T>
            {
                WasSuccess = false,
                Message = "Ya existe el registro que estas intentando crear."
            };
        }

    }
}

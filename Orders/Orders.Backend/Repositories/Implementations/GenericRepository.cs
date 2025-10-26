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
        private readonly DataContext _context; //Inyección del context.
        private readonly DbSet<T> _entity;

        //2. Constructor de la clase
        //---------------------------
        public GenericRepository(DataContext context)
        {
            _context = context;
            _entity = context.Set<T>();
        }

        //3. Métodos públicos
        //-------------------
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

        //Método de borrado
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

        //Método Get con parámetros
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

        //Método Get sin parámetros
        public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsync()
        {
            return new ActionResponse<IEnumerable<T>>
            {
                WasSuccess = true, //devuelve una lista vacía o no
                Result = await _entity.ToListAsync() // Me devuelve genericamente la lista de las entidades
            };
        }

        //Método de actualización
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

        //Añadimos los métodos de la paginación
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
        //Método con otra excepción diferente
        private ActionResponse<T> ExceptionActionResponse(Exception exception)
        {
            return new ActionResponse<T>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }

      //Método para errores en el Update de la base de datos
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

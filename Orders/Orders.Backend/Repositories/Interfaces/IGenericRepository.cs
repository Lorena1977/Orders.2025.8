using Orders.Shared.Responses;

namespace Orders.Backend.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<ActionResponse<T>> GetAsync(int id);//Le puedo mandar otro genérico (la clase Task es generico).
                                                 //Tenemos un Get del parámetro, esto es, nos devuelve un parámetro

        Task<ActionResponse<IEnumerable<T>>> GetAsync();//Nos devuelve IEnumerable(genérico) que no es más que una lista de lo que yo le pida

        Task<ActionResponse<T>> AddAsync(T entity);//Paso una entidad y lo mete en la base de datos

        Task<ActionResponse<T>> DeleteAsync(int id);//Paso un ide y lo borra de la base de datos

        Task<ActionResponse<T>> UpdateAsync(T entity);//paso un id y  me actualiza la base de datos
    }

}

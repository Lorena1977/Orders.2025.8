using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Responses;

namespace Orders.Backend.Repositories.Interfaces
{
    public interface IProductsRepository
    {
        Task<ActionResponse<Product>> GetAsync(int id); //Relaciones con categorias e imagenes cuando pido un prodcucto
        Task<ActionResponse<IEnumerable<Product>>> GetAsync(PaginationDTO pagination);
        Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);
        Task<ActionResponse<Product>> AddFullAsync(ProductDTO productDTO); //El es capaz de crear los
        //-n productos en Prodcutscategories y los n productos en Productsimages
        Task<ActionResponse<Product>> UpdateFullAsync(ProductDTO productDTO); //Lo mismo a la hora
        // de actualizar.   
        Task<ActionResponse<Product>> DeleteAsync(int id);


        Task<ActionResponse<ImageDTO>> AddImageAsync(ImageDTO imageDTO);
        Task<ActionResponse<ImageDTO>> RemoveLastImageAsync(ImageDTO imageDTO);

    }

}

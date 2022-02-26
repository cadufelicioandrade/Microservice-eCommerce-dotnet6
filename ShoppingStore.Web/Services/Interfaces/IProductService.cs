using ShoppingStore.Web.Models;

namespace ShoppingStore.Web.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductViewModel>> GetAllProduct(string token);
        Task<ProductViewModel> GetProductById(long productId, string token);
        Task<ProductViewModel> CreateProduct(ProductViewModel model, string token);
        Task<ProductViewModel> UpdateProduct(ProductViewModel model, string token);
        Task<bool> DeleteProductId(long productId, string token);

    }
}

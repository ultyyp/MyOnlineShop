using Microsoft.AspNetCore.Mvc;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.WebApi.Controllers
{
    [Route("catalog")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public CatalogController(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        [HttpPost("add_product")]
        public async Task AddProduct([FromBody] Product product, CancellationToken cancellationToken)
        {
            await _productRepository.Add(product, cancellationToken);
        }

        [HttpGet("get_products")]
        public async Task<IReadOnlyList<Product>> GetProducts(CancellationToken cancellationToken)
        {
            return await _productRepository.GetAll(cancellationToken);
        }



        [HttpGet("get_product")]
        public async Task<Product> GetProductById(Guid productId, CancellationToken cancellationToken)
        {
            return await _productRepository.GetById(productId, cancellationToken);
        }

        [HttpPost("update_product")]
        public async Task UpdateProduct([FromQuery] Guid productId,[FromBody] Product product, CancellationToken cancellationToken)
        {
            await _productRepository.UpdateProductById(productId, product, cancellationToken);
        }

        [HttpPost("delete_product")]
        public async Task DeleteProduct([FromQuery] Guid productId, CancellationToken cancellationToken)
        {
            await _productRepository.DeleteProductById(productId, cancellationToken);
        }

    }

}

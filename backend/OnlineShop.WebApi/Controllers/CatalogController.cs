using Microsoft.AspNetCore.Mvc;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Interfaces;

namespace OnlineShop.WebApi.Controllers
{
    [Route("catalog")]
    public class CatalogController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        public CatalogController(IUnitOfWork UOW)
        {
            _uow = UOW ?? throw new ArgumentNullException(nameof(UOW));
        }

        [HttpPost("add_product")]
        public async Task AddProduct([FromBody] Product product, CancellationToken cancellationToken)
        {
            await _uow.ProductRepository.Add(product, cancellationToken);
            await _uow.SaveChangesAsync();
        }

        [HttpGet("get_products")]
        public async Task<IReadOnlyList<Product>> GetProducts(CancellationToken cancellationToken)
        {
            return await _uow.ProductRepository.GetAll(cancellationToken);
        }



        [HttpGet("get_product")]
        public async Task<Product> GetProductById(Guid productId, CancellationToken cancellationToken)
        {
            return await _uow.ProductRepository.GetById(productId, cancellationToken);
        }

        [HttpPost("update_product")]
        public async Task UpdateProduct([FromQuery] Guid productId,[FromBody] Product product, CancellationToken cancellationToken)
        {
            await _uow.ProductRepository.UpdateProductById(productId, product, cancellationToken);
			await _uow.SaveChangesAsync();
		}

        [HttpPost("delete_product")]
        public async Task DeleteProduct([FromQuery] Guid productId, CancellationToken cancellationToken)
        {
            await _uow.ProductRepository.DeleteProductById(productId, cancellationToken);
			await _uow.SaveChangesAsync();
		}

    }

}

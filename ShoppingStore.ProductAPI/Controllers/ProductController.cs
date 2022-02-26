using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingStore.ProductAPI.Data.ValueObjects;
using ShoppingStore.ProductAPI.Repository;
using ShoppingStore.ProductAPI.Utils;

namespace ShoppingStore.ProductAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductController : ControllerBase
    {
        private IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new
                ArgumentNullException(nameof(productRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductVO>>> GetAll()
        {
            var products = await _productRepository.GetAll();

            if (products.Count() <= 0)
                return NotFound();

            return Ok(products);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ProductVO>> GetById(long id)
        {
            var product = await _productRepository.GetById(id);
            
            if (product == null) 
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ProductVO>> Create([FromBody] ProductVO vo)
        {
            if (vo == null) 
                return BadRequest();
            
            var productVO = await _productRepository.Create(vo);

            return CreatedAtAction("GetById", new { Id = productVO.Id }, productVO);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ProductVO>> Update([FromBody] ProductVO vo)
        {
            if (vo == null) 
                return BadRequest();
            
            var productVO = await _productRepository.Update(vo);

            return Ok(productVO);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<ActionResult> DeleteById(long id)
        {
            var status = await _productRepository.Delete(id);
            
            if (!status) 
                return NotFound(status);

            return Ok(status);
        }
    }
}
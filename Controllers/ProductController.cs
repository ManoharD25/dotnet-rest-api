using Inventory.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly ILogger<ProductsController> logger;

        public ProductsController(
            AppDbContext context,
            ILogger<ProductsController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllProducts()
        {
            logger.LogInformation("Getting all products");

            try
            {
                var products = await context.Products.ToListAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while getting products");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            logger.LogInformation("Getting product by id");

            try
            {
                var product = await context.Products.FindAsync(id);
                if (product == null)
                    return NotFound();

                return Ok(product);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while getting products");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            logger.LogInformation("Creating new product");

            if (string.IsNullOrWhiteSpace(product.Name))
                return BadRequest("Product name is required");

            //product.Id = Guid.NewGuid();

            context.Products.Add(product);
            await context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetProductById),
                new { id = product.Id },
                product
            );
        }
    }
}

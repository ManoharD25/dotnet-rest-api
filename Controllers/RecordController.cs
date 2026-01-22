using Inventory.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecordController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly ILogger<RecordController> logger;

        public RecordController(
            AppDbContext context,
            ILogger<ProductsController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<Record>>> GetAllrecords()
        {
            logger.LogInformation("Getting all records");

            try
            {
                var records = await context.Records.ToListAsync();
                return Ok(records);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while getting records");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using RestApiLK.data;
using RestApiLK.services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestApiLK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdukterController : ControllerBase
    {
        private readonly ProduktService _produktService;

        public ProdukterController(ProduktService produktService)
        {
            _produktService = produktService;
        }

        // GET: api/Produkt
        [HttpGet]
        public async Task<ActionResult<IEnumerable<produkter>>> Get()
        {
            var produkter = await _produktService.GetAllAsync();
            return Ok(produkter);
        }

        // GET: api/Produkt/5
        [HttpGet("{id}")]
        public async Task<ActionResult<produkter>> Get(int id)
        {
            var produkt = await _produktService.GetByIdAsync(id);
            if (produkt == null)
            {
                return NotFound();
            }
            return Ok(produkt);
        }

        // POST: api/Produkt
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] produkter produkt)
        {
            var createdProdukt = await _produktService.InsertAsync(produkt);
            if (createdProdukt == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(Get), new { id = createdProdukt.ProduktID }, createdProdukt);
        }

        // PUT: api/Produkt/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] produkter produkt)
        {
            if (id != produkt.ProduktID)
            {
                return BadRequest();
            }

            var updatedProdukt = await _produktService.UpdateAsync(produkt);
            if (updatedProdukt == null)
            {
                return NotFound();
            }
            return Ok(updatedProdukt);
        }

        // DELETE: api/Produkt/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _produktService.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}


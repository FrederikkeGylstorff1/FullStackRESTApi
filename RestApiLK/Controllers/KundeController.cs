using Microsoft.AspNetCore.Mvc;
using RestApiLK.data;
using RestApiLK.services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestApiLK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KundeController : ControllerBase
    {
        private readonly kundeService _kundeService;

        public KundeController(kundeService kundeService)
        {
            _kundeService = kundeService;
        }

        // GET: api/Kunde
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kunder>>> Get()
        {
            var kunder = await _kundeService.GetAllAsync();
            return Ok(kunder);
        }

        // GET: api/Kunde/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Kunder>> Get(int id)
        {
            var kunde = await _kundeService.GetByIdAsync(id);
            if (kunde == null)
            {
                return NotFound();
            }
            return Ok(kunde);
        }

        // POST: api/Kunde
        // POST: api/Kunde
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Kunder kunde)
        {
            var createdKunde = await _kundeService.InsertAsync(kunde);
            if (createdKunde == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(Get), new { id = createdKunde.KundeID }, createdKunde);
        }


        // PUT: api/Kunde/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Kunder kunde)
        {
            if (id != kunde.KundeID)
            {
                return BadRequest();
            }

            var updatedKunde = await _kundeService.UpdateAsync(kunde);
            if (updatedKunde == null)
            {
                return NotFound();
            }
            return Ok(updatedKunde);
        }


        // DELETE: api/Kunde/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _kundeService.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}

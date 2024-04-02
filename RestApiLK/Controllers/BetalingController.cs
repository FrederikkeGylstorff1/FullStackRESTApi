using Microsoft.AspNetCore.Mvc;
using RestApiLK.data;
using RestApiLK.services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestApiLK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetalingController : ControllerBase
    {
        private readonly BetalingService _betalingService;

        public BetalingController(BetalingService betalingService)
        {
            _betalingService = betalingService;
        }

        // GET: api/Betaling
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Betaling>>> Get()
        {
            var betalinger = await _betalingService.GetAllAsync();
            return Ok(betalinger);
        }

        // GET: api/Betaling/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Betaling>> Get(int id)
        {
            var betaling = await _betalingService.GetByIdAsync(id);
            if (betaling == null)
            {
                return NotFound();
            }
            return Ok(betaling);
        }


        // POST: api/Betaling
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Betaling betaling)
        {
            var createBetaling = await _betalingService.InsertAsync(betaling);
            if (createBetaling == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(Get), new { id = createBetaling.BetalingsID }, createBetaling);
        }


        // PUT: api/Betaling/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Betaling betaling)
        {
            if (id != betaling.BetalingsID)
            {
                return BadRequest();
            }

            var updatedKunde = await _betalingService.UpdateAsync(betaling);
            if (updatedKunde == null)
            {
                return NotFound();
            }
            return Ok(updatedKunde);
        }


        // DELETE: api/Betaling/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _betalingService.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using RestApiLK.data;
using RestApiLK.services;

namespace RestApiLK.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class OrdreController : Controller
    {
        private readonly OrdreService _ordreService;


        public OrdreController(OrdreService ordreService)
        {
                _ordreService = ordreService;
        }

        // GET: /
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ordre>>> Get()
        {
            var Ordrer = await _ordreService.GetAllAsync();
            return Ok(Ordrer);
        }

        // GET: api/Ordre/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ordre>> Get(int id)
        {
            var ordre = await _ordreService.GetByIdAsync(id);
            if (ordre == null)
            {
                return NotFound();
            }
            return Ok(ordre);
        }

        // POST: api/Ordre
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Ordre ordre)
        {
            var created = await _ordreService.InsertAsync(ordre);
            if (created == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(Get), new { id = created.KundeID }, created);
        }


        // PUT: api/Ordre/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Ordre ordre)
        {
            if (id != ordre.OrdreID)
            {
                return BadRequest();
            }

            var updated = await _ordreService.UpdateAsync(ordre);
            if (updated == null)
            {
                return NotFound();
            }
            return Ok(updated);
        }


        // DELETE: api/Ordre/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _ordreService.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}

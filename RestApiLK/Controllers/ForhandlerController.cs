using Microsoft.AspNetCore.Mvc;
using RestApiLK.data;
using RestApiLK.services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestApiLK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForhandlerController : ControllerBase
    {
        private readonly ForhandlerService _forhandlerService;

        public ForhandlerController(ForhandlerService forhandlerService)
        {
            _forhandlerService = forhandlerService;
        }

        // GET: api/Forhandler
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Forhandler>>> Get()
        {
            var forhandlere = await _forhandlerService.GetAllAsync();
            return Ok(forhandlere);
        }

        // GET: api/Forhandler/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Forhandler>> Get(int id)
        {
            var forhandler = await _forhandlerService.GetByIdAsync(id);
            if (forhandler == null)
            {
                return NotFound();
            }
            return Ok(forhandler);
        }

        // POST: api/Forhandler
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Forhandler forhandler)
        {
            var createdForhandler = await _forhandlerService.InsertAsync(forhandler);
            if (createdForhandler == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(Get), new { id = createdForhandler.ForhandlerID }, createdForhandler);
        }

        // PUT: api/Forhandler/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Forhandler forhandler)
        {
            if (id != forhandler.ForhandlerID)
            {
                return BadRequest();
            }

            var updatedForhandler = await _forhandlerService.UpdateAsync(forhandler);
            if (updatedForhandler == null)
            {
                return NotFound();
            }
            return Ok(updatedForhandler);
        }

        // DELETE: api/Forhandler/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _forhandlerService.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}

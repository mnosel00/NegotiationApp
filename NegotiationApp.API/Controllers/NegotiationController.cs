using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NegotiationApp.Application.DTOs;
using NegotiationApp.Application.Interfaces;

namespace NegotiationApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NegotiationController : ControllerBase
    {
        private readonly INegotiationService _negotiationService;

        public NegotiationController(INegotiationService negotiationService)
        {
            _negotiationService = negotiationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NegotiationDto>>> GetAllNegotiations()
        {
            var negotiations = await _negotiationService.GetAllNegotiationsAsync();
            return Ok(negotiations);
        }

        [HttpPost]
        public async Task<IActionResult> AddNegotiation([FromBody] CreateNegotiationDto negotiationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _negotiationService.AddNegotiationAsync(negotiationDto);
            return CreatedAtAction(nameof(GetNegotiationById), new { id = negotiationDto.ProductId }, negotiationDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NegotiationDto>> GetNegotiationById(int id)
        {
            var negotiation = await _negotiationService.GetNegotiationByIdAsync(id);
            if (negotiation == null)
            {
                return NotFound();
            }

            return Ok(negotiation);
        }

        [HttpPut("{id}/propose")]
        public async Task<IActionResult> ProposeNewPrice(int id, [FromBody] decimal newPrice)
        {
            try
            {
                await _negotiationService.ProposeNewPriceAsync(id, newPrice);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/accept")]
        public async Task<IActionResult> AcceptNegotiation(int id)
        {
            try
            {
                await _negotiationService.AcceptNegotiationAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/reject")]
        public async Task<IActionResult> RejectNegotiation(int id)
        {
            try
            {
                await _negotiationService.RejectNegotiationAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/check-expiration")]
        public async Task<ActionResult<TimeSpan>> CheckExpiration(int id)
        {
            try
            {
                var timeRemaining = await _negotiationService.CheckExpirationAsync(id);
                return Ok(timeRemaining);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}

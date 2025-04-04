using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<IEnumerable<NegotiationDto>>> GetAllNegotiations()
        {
            var negotiations = await _negotiationService.GetAllNegotiationsAsync();
            
            return Ok(negotiations);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddNegotiation([FromBody] CreateNegotiationDto negotiationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (isSuccess, errorMessage) = await _negotiationService.AddNegotiationAsync(negotiationDto);
            
            if (!isSuccess)
            {
                return BadRequest(new { message = errorMessage });
            }

            return CreatedAtAction(nameof(GetNegotiationById), new { id = negotiationDto.ProductId }, negotiationDto);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminPolicy")]
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
        [AllowAnonymous]
        public async Task<IActionResult> ProposeNewPrice(int id, [FromBody] decimal newPrice)
        {
            var (isSuccess, errorMessage) = await _negotiationService.ProposeNewPriceAsync(id, newPrice);
            
            if (!isSuccess)
            {
                return BadRequest(new { message = errorMessage });
            }

            return NoContent();
        }

        [HttpPut("{id}/accept")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> AcceptNegotiation(int id)
        {
            var (isSuccess, errorMessage) = await _negotiationService.AcceptNegotiationAsync(id);
            
            if (!isSuccess)
            {
                return BadRequest(new { message = errorMessage });
            }

            return NoContent();
        }

        [HttpPut("{id}/reject")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> RejectNegotiation(int id)
        {
            var (isSuccess, errorMessage) = await _negotiationService.RejectNegotiationAsync(id);
            
            if (!isSuccess)
            {
                return BadRequest(new { message = errorMessage });
            }

            return NoContent();
        }

        [HttpPut("{id}/check-expiration")]
        [Authorize(Policy = "AdminPolicy")]
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

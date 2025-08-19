using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StatSanctum.Entities;
using StatSanctum.Handlers;
using StatSanctum.Models;

namespace StatSanctum.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RarityController : ControllerBase
    {
        private IMediator _mediator;
        private IMapper _mapper;
        public RarityController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet(Name = "GetRarities")]
        public async Task<ActionResult> GetAllRarity()
        {
            try
            {
                var rarities = await _mediator.Send(new GetAllQuery<Rarity>());

                return Ok(rarities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}", Name = "GetRaritiesById")]
        public async Task<ActionResult> GetRaritiesById(int id)
        {
            try
            {
                var rarity = await _mediator.Send(new GetByIdQuery<Rarity> { Id = id });

                return Ok(rarity);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost(Name = "CreateRarity")]
        public async Task<ActionResult> CreateRarity([FromBody] RarityDto rarity)
        {
            if (rarity == null)
            {
                return BadRequest("Rarity data is required.");
            }
            try
            {
                var savedRarity = await _mediator.Send(new CreateCommand<Rarity> { Entity = _mapper.Map<Rarity>(rarity) });

                return CreatedAtAction(nameof(GetRaritiesById), new { id = savedRarity.RarityID }, savedRarity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}", Name = "UpdateRarityById")]
        public async Task<ActionResult> UpdateRarityById(int id, [FromBody] RarityDto rarityDto)
        {
            if (rarityDto == null)
            {
                return BadRequest("Rarity data is required.");
            }
            try
            {
                // Get existing item
                var existingRarity = await _mediator.Send(new GetByIdQuery<Rarity> { Id = id });

                var updatedRarity = _mapper.Map(rarityDto, existingRarity);

                var rarityEquipment = await _mediator.Send(new UpdateCommand<Rarity> { Entity = updatedRarity });

                return Ok(rarityEquipment); // 200 OK
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // 404 Not Found
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}", Name = "DeleteRarityById")]
        public async Task<ActionResult> DeleteRarityById(int id)
        {
            try
            {
                await _mediator.Send(new DeleteCommand<Rarity> { Id = id });

                return NoContent(); // 204 No Content
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // 404 Not Found
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

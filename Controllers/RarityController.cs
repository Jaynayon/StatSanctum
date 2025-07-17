using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StatSanctum.Entities;
using StatSanctum.Handlers;
using StatSanctum.Models;
using StatSanctum.Repositories;

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
        public async Task<ActionResult<IEnumerable<Rarity>>> GetAllRarity()
        {
            try
            {
                var query = new GetAllQuery<Rarity>();
                var rarities = await _mediator.Send(query);

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
                var query = new GetByIdQuery<Rarity> { Id = id };
                var rarity = await _mediator.Send(query);

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
                var query = new CreateCommand<Rarity> { Entity = _mapper.Map<Rarity>(rarity) };
                var savedRarity = await _mediator.Send(query);

                return CreatedAtAction(nameof(GetRaritiesById), new { id = savedRarity.RarityID }, savedRarity);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}", Name = "UpdateRarityById")]
        public async Task<ActionResult<Rarity>> UpdateRarityById(int id, [FromBody] RarityDto rarityDto)
        {
            if (rarityDto == null)
            {
                return BadRequest("Rarity data is required.");
            }
            try
            {
                // Get existing item
                var query = new GetByIdQuery<Rarity> { Id = id };
                var existingRarity = await _mediator.Send(query);

                var updatedRarity = _mapper.Map(rarityDto, existingRarity);

                var command = new UpdateCommand<Rarity> { Entity = updatedRarity };
                var rarityEquipment = await _mediator.Send(command);

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
                var command = new DeleteCommand<Rarity> { Id = id };
                await _mediator.Send(command);

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

using Microsoft.AspNetCore.Mvc;
using StatSanctum.Entities;
using StatSanctum.Models;
using StatSanctum.Repositories;

namespace StatSanctum.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RarityController : ControllerBase
    {
        private readonly IRarityRepository _rarityRepository;
        public RarityController(IRarityRepository rarityRepository)
        {
            _rarityRepository = rarityRepository ?? throw new ArgumentNullException(nameof(rarityRepository));
        }
        [HttpGet(Name = "GetRarities")]
        public async Task<ActionResult<IEnumerable<Rarity>>> GetRarities()
        {
            try
            {
                var rarity = await _rarityRepository.GetAll();
                return Ok(rarity);
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
                var rarity = await _rarityRepository.GetById(id);

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
                var savedRarity = await _rarityRepository.Create(rarity);

                return CreatedAtAction(nameof(GetRarities), new { id = savedRarity.RarityID }, savedRarity);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}", Name = "UpdateRarityById")]
        public async Task<ActionResult<Item>> UpdateRarityById(int id, [FromBody] RarityDto rarityDto)
        {
            if (rarityDto == null)
            {
                return BadRequest("Rarity data is required.");
            }
            try
            {
                var rarityEquipment = await _rarityRepository.Update(id, rarityDto);
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
                await _rarityRepository.DeleteById(id);
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

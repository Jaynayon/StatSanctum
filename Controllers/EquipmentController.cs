using Microsoft.AspNetCore.Mvc;
using StatSanctum.Entities;
using StatSanctum.Models;
using StatSanctum.Repositories;

namespace StatSanctum.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EquipmentController : ControllerBase
    {
        private readonly IEquipmentRepository _equipmentRepository;
        public EquipmentController(IEquipmentRepository equipmentRepository)
        {
            _equipmentRepository = equipmentRepository ?? throw new ArgumentNullException(nameof(equipmentRepository));
        }

        [HttpGet(Name = "GetEquipments")]
        public async Task<ActionResult<IEnumerable<Equipment>>> GetEquipments()
        {
            try
            {
                var equipments = await _equipmentRepository.GetAll();
                return Ok(equipments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}",Name = "GetEquipmentsById")]
        public async Task<ActionResult> GetEquipmentsById(int id)
        {
            try
            {
                var equipment = await _equipmentRepository.GetById(id);
      
                return Ok(equipment);
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost(Name = "CreateEquipment")]
        public async Task<ActionResult> CreateEquipment([FromBody] EquipmentDto equipment)
        {
            if (equipment == null)
            {
                return BadRequest("Equipment data is required.");
            }
            try
            {
                var savedEquipment = await _equipmentRepository.Create(equipment);

                return CreatedAtAction(nameof(GetEquipments), new { id = savedEquipment.Id }, savedEquipment);

            } 
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}", Name = "UpdateEquipmentById")]
        public async Task<ActionResult<Equipment>> UpdateEquipment(int id, [FromBody] EquipmentDto equipmentDto)
        {
            if (equipmentDto == null)
            {
                return BadRequest("Equipment data is required.");
            }
            try
            {
                var updatedEquipment = await _equipmentRepository.Update(id, equipmentDto);
                return Ok(updatedEquipment); // 200 OK
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

        [HttpDelete("{id}", Name = "DeleteEquipmentById")]
        public async Task<ActionResult> DeleteEquipmentsById(int id)
        {
            try
            {
                await _equipmentRepository.DeleteById(id);
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

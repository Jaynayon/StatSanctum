using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StatSanctum.Contexts;
using StatSanctum.Entities;
using StatSanctum.Handlers;
using StatSanctum.Models;
using StatSanctum.Repositories;

namespace StatSanctum.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private IMediator _mediator;
        private IMapper _mapper;

        public ItemController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet(Name = "GetAllItems")]
        public async Task<ActionResult<IEnumerable<Item>>> GetAllItems()
        {
            try
            {
                var query = new GetAllQuery<Item>();
                var items = await _mediator.Send(query);
            
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}", Name = "GetItemsById")]
        public async Task<ActionResult> GetItemsById(int id)
        {
            try
            {
                var query = new GetByIdQuery<Item> { Id = id };
                var item = await _mediator.Send(query);

                return Ok(item);
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

        [HttpPost(Name = "CreateItem")]
        public async Task<ActionResult> CreateItem([FromBody] ItemDto equipment)
        {
            if (equipment == null)
            {
                return BadRequest("Equipment data is required.");
            }
            try
            {
                var command = new CreateCommand<Item> { Entity = _mapper.Map<Item>(equipment) };
                var savedEquipment = await _mediator.Send(command);

                return CreatedAtAction(nameof(GetItemsById), new { id = savedEquipment.ItemID }, savedEquipment);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}", Name = "UpdateItemsById")]
        public async Task<ActionResult<Item>> UpdateItemsById(int id, [FromBody] ItemDto itemDto)
        {
            if (itemDto == null)
            {
                return BadRequest("Equipment data is required.");
            }
            try
            {
                // Get existing item
                var query = new GetByIdQuery<Item> { Id = id };
                var item = await _mediator.Send(query);

                var updatedItem = _mapper.Map(itemDto, item);

                var command = new UpdateCommand<Item> { Entity = updatedItem };
                var updatedEquipment = await _mediator.Send(command);

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

        [HttpDelete("{id}", Name = "DeleteItemsById")]
        public async Task<ActionResult> DeleteItemsById(int id)
        {
            try
            {
                var command = new DeleteCommand<Item> { Id = id };
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

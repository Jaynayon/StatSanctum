using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StatSanctum.Entities;
using StatSanctum.Handlers;
using StatSanctum.Models;
using StatSanctum.Queries.Items;

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
        public async Task<ActionResult> GetAllItems()
        {
            try
            {
                var items = await _mediator.Send(new GetAllItemsQuery());
            
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
                var item = await _mediator.Send(new GetItemByIdQuery { Id = id });

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
                var savedEquipment = await _mediator.Send(new CreateCommand<Item> { Entity = _mapper.Map<Item>(equipment) });

                return CreatedAtAction(nameof(GetItemsById), new { id = savedEquipment.ItemID }, savedEquipment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}", Name = "UpdateItemsById")]
        public async Task<ActionResult> UpdateItemsById(int id, [FromBody] ItemDto itemDto)
        {
            if (itemDto == null)
            {
                return BadRequest("Equipment data is required.");
            }
            try
            {
                // Get existing item
                var item = await _mediator.Send(new GetByIdQuery<Item> { Id = id });

                var updatedItem = _mapper.Map(itemDto, item);

                var updatedEquipment = await _mediator.Send(new UpdateCommand<Item> { Entity = updatedItem });

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
                await _mediator.Send(new DeleteCommand<Item> { Id = id });

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

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
    public class ItemTypeController : ControllerBase
    {
        private IMediator _mediator;
        private IMapper _mapper;

        public ItemTypeController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet(Name = "GetAllItemTypes")]
        public async Task<ActionResult<IEnumerable<ItemType>>> GetAllItemTypes()
        {
            try
            {
                var type = await _mediator.Send(new GetAllQuery<ItemType>());

                return Ok(type);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}", Name = "GetItemTypesById")]
        public async Task<ActionResult<ItemType>> GetItemTypesById(int id)
        {
            try
            {
                var type = await _mediator.Send(new GetByIdQuery<ItemType> { Id = id });

                return Ok(type);
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

        [HttpPost(Name = "CreateItemType")]
        public async Task<ActionResult> CreateItemType([FromBody] ItemTypeDto type)
        {
            if (type == null)
            {
                return BadRequest("Item Type data is required.");
            }
            try
            {
                var savedItemType = await _mediator.Send(new CreateCommand<ItemType> { Entity = _mapper.Map<ItemType>(type) });

                return CreatedAtAction(nameof(GetItemTypesById), new { id = savedItemType.ItemTypeID }, savedItemType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}", Name = "UpdateItemTypesById")]
        public async Task<ActionResult<Item>> UpdateItemTypesById(int id, [FromBody] ItemTypeDto itemTypeDto)
        {
            if (itemTypeDto == null)
            {
                return BadRequest("Item Type data is required.");
            }
            try
            {
                // Get existing item
                var item = await _mediator.Send(new GetByIdQuery<ItemType> { Id = id });

                var updatedEntity = _mapper.Map(itemTypeDto, item);

                var updatedItemType = await _mediator.Send(new UpdateCommand<ItemType> { Entity = updatedEntity });

                return Ok(updatedItemType); // 200 OK
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

        [HttpDelete("{id}", Name = "DeleteItemTypesById")]
        public async Task<ActionResult> DeleteItemTypesById(int id)
        {
            try
            {
                await _mediator.Send(new DeleteCommand<ItemType> { Id = id });

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

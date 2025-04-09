using Microsoft.AspNetCore.Mvc;
using EliteAI.Application.Interfaces;

namespace EliteAI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController<T> : ControllerBase where T : class
{
    protected readonly IService<T> _service;
    protected readonly ILogger<BaseController<T>> _logger;

    protected BaseController(IService<T> service, ILogger<BaseController<T>> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public virtual async Task<ActionResult<IEnumerable<T>>> GetAll()
    {
        try
        {
            var entities = await _service.GetAllAsync();
            return Ok(entities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all entities");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public virtual async Task<ActionResult<T>> GetById(Guid id)
    {
        try
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting entity by id");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public virtual async Task<ActionResult<T>> Create(T entity)
    {
        try
        {
            var createdEntity = await _service.CreateAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = GetEntityId(createdEntity) }, createdEntity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating entity");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public virtual async Task<ActionResult<T>> Update(Guid id, T entity)
    {
        try
        {
            var updatedEntity = await _service.UpdateAsync(id, entity);
            return Ok(updatedEntity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating entity");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public virtual async Task<ActionResult> Delete(Guid id)
    {
        try
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting entity");
            return StatusCode(500, "Internal server error");
        }
    }

    protected abstract Guid GetEntityId(T entity);
} 
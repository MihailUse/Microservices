using AutoMapper;
using EquipmentMicroservice.DAL;
using EquipmentMicroservice.DAL.Entities;
using EquipmentMicroservice.Models;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentMicroservice.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EquipmentController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly DataContext _context;

    public EquipmentController(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    /// <summary>
    /// returns list of equipment
    /// </summary>
    /// <returns>list of equipment</returns>
    [HttpGet]
    public IEnumerable<Equipment> Get()
    {
        return _context.Equipments;
    }

    /// <summary>
    /// Return equipment by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Equipment</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Equipment))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid id)
    {
        var equipment = await _context.Equipments.FindAsync(id);
        if (equipment == default)
            return NotFound();

        return Ok(equipment);
    }

    /// <summary>
    /// Create new equipment
    /// </summary>
    /// <param name="createEquipmentModel"></param>
    /// <returns>New equipment</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Equipment))]
    public async Task<IActionResult> Post([FromBody] CreateEquipmentModel createEquipmentModel)
    {
        var equipment = _mapper.Map<Equipment>(createEquipmentModel);
        await _context.Equipments.AddAsync(equipment);
        await _context.SaveChangesAsync();
        return Ok(equipment);
    }

    /// <summary>
    /// Not implemented
    /// </summary>
    /// <param name="id"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Put(Guid id, [FromBody] string value)
    {
        return BadRequest("Not implemented");
    }

    /// <summary>
    /// Delete equipment by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var equipment = await _context.Equipments.FindAsync(id);
        if (equipment == default)
            return NotFound();

        _context.Equipments.Remove(equipment);
        await _context.SaveChangesAsync();
        return Ok();
    }
}
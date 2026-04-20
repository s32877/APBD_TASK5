using APBD_TASK5.Models;
using Microsoft.AspNetCore.Mvc;

namespace APBD_TASK5.Controllers;
[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    [HttpGet]
    public ActionResult<List<Room>> GetAll(
        [FromQuery] int? minCapacity,
        [FromQuery] bool? hasProjector,
        [FromQuery] bool? activeOnly
        )
    {
        var rooms = Database.DataStore.Rooms.AsEnumerable();
        if (minCapacity.HasValue)
            rooms = rooms.Where(r => r.Capacity >= minCapacity.Value);
        if (hasProjector.HasValue)
            rooms = rooms.Where(r => r.HasProjector == hasProjector.Value);
        if (activeOnly.HasValue)
            rooms = rooms.Where(r => r.IsActive);
        return Ok(rooms.ToList());
    }

    [HttpGet("{id:int}")]
    public ActionResult<Room> GetById(int id)
    {
        var room = Database.DataStore.Rooms.FirstOrDefault(r => r.Id == id);
        if (room == null)
        {
            return NotFound($"Room with id {id} not found");
        }

        return Ok(room);
        
    }

    [HttpPost]
    public ActionResult<Room> Create(Room room)
    {
        room.Id = Database.DataStore.NextRoomId;
        Database.DataStore.Rooms.Add(room);
        return  CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
    }

    [HttpGet("building/{buildingCode}")]
    public ActionResult<Room> GetByBuilding(string buildingCode)
    {
        var rooms = Database.DataStore.Rooms
            .Where(r => r.BuildingCode.Equals(buildingCode, StringComparison.OrdinalIgnoreCase))
            .ToList();
        return Ok(rooms);
    }

    [HttpPut("{id:int}")]
    public ActionResult<Room> UpdateRoom(int id, Room room)
    {
        var rooms = Database.DataStore.Rooms.FirstOrDefault(r => r.Id == id);
        if (rooms == null)
            return NotFound($"Room with id {id} doesn't exist");
        rooms.Name = room.Name;
        rooms.Capacity = room.Capacity;
        rooms.BuildingCode = room.BuildingCode;
        rooms.HasProjector = room.HasProjector;
        rooms.IsActive = room.IsActive;
        return Ok(rooms);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<Room> DeleteRoom(int id)
    {
        var room = Database.DataStore.Rooms.FirstOrDefault(r=>r.Id == id);
        if (room == null)
            return NotFound($"Room with id {id} not found");

        Database.DataStore.Rooms.Remove(room);
        return NoContent();
    }
}
using APBD_TASK5.Models;
using Microsoft.AspNetCore.Mvc;

namespace APBD_TASK5.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ReservationController : ControllerBase
{
    [HttpGet]
    public ActionResult<List<Reservation>> GetAll(
        [FromQuery] DateOnly? date,
        [FromQuery] string? status,
        [FromQuery] int? roomId)
    {
        var reservations = Database.DataStore.Reservations.AsEnumerable();
        if (date.HasValue)
            reservations = reservations.Where(r => r.Date == date.Value);

        if (!string.IsNullOrEmpty(status))
            reservations = reservations.Where(r => r.Status.Equals(status, StringComparison.OrdinalIgnoreCase));

        if (roomId.HasValue)
            reservations = reservations.Where(r => r.RoomId == roomId.Value);
        return Ok(reservations.ToList());
    }
    [HttpGet("{id:int}")]
    public ActionResult<Reservation> GetById([FromRoute] int id)
    {
        var reservation = Database.DataStore.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation == null)
            return NotFound($"Reservation with id {id} not found");

        return Ok(reservation);
    }
    [HttpPost]
    public ActionResult<Reservation> CreateReservation(Reservation reservation)
    {
        var roomExists = Database.DataStore.Rooms.Any(r => r.Id == reservation.RoomId);
        if (!roomExists)
            return BadRequest($"Room with id {reservation.RoomId} does not exist");
        if (reservation.EndTime <= reservation.StartTime)
            return BadRequest("EndTime must be later than StartTime.");

        reservation.Id = Database.DataStore.NextReservationId;
        Database.DataStore.Reservations.Add(reservation);
        return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
    }
    [HttpPut("{id:int}")]
    public ActionResult<Reservation> Update([FromRoute] int id, [FromBody] Reservation reservation)
    {
        var existing = Database.DataStore.Reservations.FirstOrDefault(r => r.Id == id);
        if (existing == null)
            return NotFound($"Reservation with id {id} not found");

        var roomExists = Database.DataStore.Rooms.Any(r => r.Id == reservation.RoomId);
        if (!roomExists)
            return BadRequest($"Room with id {reservation.RoomId} does not exist");

        existing.RoomId = reservation.RoomId;
        existing.OrganizerName = reservation.OrganizerName;
        existing.Topic = reservation.Topic;
        existing.Date = reservation.Date;
        existing.StartTime = reservation.StartTime;
        existing.EndTime = reservation.EndTime;
        existing.Status = reservation.Status;

        return Ok(existing);
    }
    [HttpDelete("{id:int}")]
    public ActionResult Delete([FromRoute] int id)
    {
        var reservation = Database.DataStore.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation == null)
            return NotFound($"Reservation with id {id} not found");

        Database.DataStore.Reservations.Remove(reservation);
        return NoContent();
    }
}
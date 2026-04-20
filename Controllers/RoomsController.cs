using APBD_TASK5.Models;
using Microsoft.AspNetCore.Mvc;

namespace APBD_TASK5.Controllers;
[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    [HttpGet]
    public ActionResult<List<Room>> GetAll()
    {
        var rooms = Database.DataStore.Rooms.AsEnumerable();
        return Ok(rooms.ToList());
    }
    
}
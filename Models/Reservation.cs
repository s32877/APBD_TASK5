using System.ComponentModel.DataAnnotations;

namespace APBD_TASK5.Models;

public class Reservation
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    [Required]
    [MinLength(1)]
    public string OrganizerName  { get; set; } = string.Empty;
    [Required]
    [MinLength(1)]
    public string Topic { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    [Required]
    public TimeOnly EndTime { get; set; }
    public string Status { get; set; }  = string.Empty;
}
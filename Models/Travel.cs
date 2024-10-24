namespace MuevemeApi.Models;

public class Travel 
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int VehicleId { get; set; } 
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string FromCoordinates { get; set; } = "";
    public string ToCoordinates { get; set; }  = "";
    public decimal Price { get; set; }
}
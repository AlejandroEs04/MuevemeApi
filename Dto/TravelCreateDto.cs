namespace MuevemeApi.Dtos;

public class TravelCreateDto
{
    public int UserId { get; set; }
    public string VehicleId { get; set; } = "";
    public string StartTime { get; set; } = "";
    public string EndTime { get; set; } = "";
    public string FromCoordinates { get; set; } = "";
    public string ToCoordinates { get; set; }  = "";
    public decimal Price { get; set; }
}
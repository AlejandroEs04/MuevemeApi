namespace MuevemeApi.Dtos;

public class TravelUpdateDto
{
    public int VehicleId { get; set; }
    public string StartTime { get; set; } = "";
    public string EndTime { get; set; } = "";
    public string FromCoordinates { get; set; } = "";
    public string ToCoordinates { get; set; }  = "";
    public decimal Price { get; set; }
    public int PresetMode { get; set; }
    public IEnumerable<int> ActiveDay { get; set; } = [];
}
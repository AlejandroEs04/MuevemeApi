namespace MuevemeApi.Models;

public class Request
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int TravelId { get; set; }
    public bool Status { get; set; }
    public DateTime Date { get; set; }
}
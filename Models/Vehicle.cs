namespace MuevemeApi.Models;

public class Vehicle 
{
    public string Id { get; set; } = "";
    public int UserId { get; set; }
    public string Model { get; set; } = "";
    public string Color { get; set; } = "";
    public string Year { get; set; } = "";
}
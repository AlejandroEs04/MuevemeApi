namespace MuevemeApi.Dtos;

public class UserGetDto
{
    public int Id { get; set;}
    public string Name { get; set;} = "";
    public string LastName { get; set;} = "";
    public string Email { get; set;} = "";
    public string PhoneNumber { get; set;} = "";
    public string UserName { get; set;} = "";
    public string PorfileImageUrl { get; set;} = "";
}
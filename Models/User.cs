namespace MuevemeApi.Models;

public class User 
{
    public int Id { get; set;}
    public string Name { get; set;} = "";
    public string LastName { get; set;} = "";
    public string Email { get; set;} = "";
    public string PhoneNumber { get; set;} = "";
    public string UserName { get; set;} = "";
    public byte[] PasswordHash { get; set;} = new byte[0];
    public byte[] PasswordSalt { get; set;} = new byte[0];
    public string ProfileImageUrl { get; set;} = "";
}
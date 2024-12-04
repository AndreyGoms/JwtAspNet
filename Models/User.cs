namespace JwtAspNet.Models;

public record User(
    int Id,
    string Name,
    string Image,
    string Email, 
    string Password, 
    string[] Roles);
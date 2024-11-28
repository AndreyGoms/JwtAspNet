using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace JwtAspNet.Services;

public class TokenService 
{
    public string Create()
    {
        var handler = new JwtSecurityTokenHandler();
        
        var key = Encoding.ASCII.GetBytes(Configuration.PrivateKey);

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor() 
        {
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.AddHours(2) //Tempo não tão curto p/ ser chato ao usuario e nem tao longo para não ter perigo de roubo
        };   
        

        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }
}
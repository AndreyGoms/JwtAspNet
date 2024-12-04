using System.Security.Claims;
using System.Text;
using JwtAspNet;
using JwtAspNet.Extensions;
using JwtAspNet.Models;
using JwtAspNet.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<TokenService>();
/*
    Sempre nessa ordem, pois precisa-se saber quem o usuario é para depois 
    saber o que ele pode fazer!
*/
builder.Services.AddAuthentication(x => {
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; 
})
.AddJwtBearer (x => 
        {
            x.TokenValidationParameters = new TokenValidationParameters(){
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.PrivateKey)),
            ValidateIssuer = false,
            ValidateAudience = false
            };
        }
    );

builder.Services.AddAuthorization();

//a mesma regra da ordem segue aqui
var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/login", (TokenService service) => 
{
    return service.Create(
        new User(
            1,
             "Andrey Gomes",
             "imagem",
             "andrey@email.com",
             "1234",
             new [] {"Student", "Premium"}
        )
    );
});

//[Authorize]
app.MapGet("/restrito", () => "Você possui acesso!")
   .RequireAuthorization();

app.MapGet("/restrito2", (ClaimsPrincipal user) => new
    {
        id = user.Id(),
        name = user.Name(),
        email = user.Email(),
        givenName = user.GivenName(),
        image = user.Image(),
    })
   .RequireAuthorization();

app.Run();

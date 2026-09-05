using MS.Application.Contracts.authentication;
using MS.Application.Contracts.User.DTOs;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;


namespace MS.Application.Services;

public class TokenService : ITokenService
{
    public string GenerateToken(UserViewDTO user, string secret, string issuer, string audience)
    {
        List<Claim> claims = new()
        {
            new Claim("userId" , user.UserId.ToString()),
            new Claim(ClaimTypes.Email , user.UserEmail),
            new Claim(ClaimTypes.Role , user.UserRoleName),
            new Claim("UserPassword" , user.UserPassword),
        };
        var key = secret;
        var secretkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var Credential = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
                   issuer: issuer,
                   audience: audience,
                   claims: claims,
                   expires: DateTime.Now.AddMinutes(30),
                   signingCredentials: Credential
               );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
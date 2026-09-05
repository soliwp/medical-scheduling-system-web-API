using MS.Application.Contracts.User.DTOs;

namespace MS.Application.Contracts.authentication;
public interface ITokenService
{
    string GenerateToken(UserViewDTO user,string secret , string issuer ,string audience);
}

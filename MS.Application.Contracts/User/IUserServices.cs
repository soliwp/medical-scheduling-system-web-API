using MS.Application.Contracts.Helpers;
using MS.Application.Contracts.User.DTOs;

namespace MS.Application.Contracts.User;
public interface IUserServices
{
    Task<APIResponse<UserViewDTO>> Login(UserLoginDTO model);    
}

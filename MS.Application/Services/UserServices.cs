using MS.Application.Contracts.Helpers;
using MS.Application.Contracts.User;
using MS.Application.Contracts.User.DTOs;
using MS.Domain.Entities.authentication;
using System.Net;

namespace MS.Application.Services;

public class UserServices : IUserServices
{
    private readonly IUserRepository _userRepository;

    public UserServices(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<APIResponse<UserViewDTO>> Login(UserLoginDTO model)
    {
        try
        {
            if(model is null)
            {
                return new APIResponse<UserViewDTO>(400,ErrorMessages.ValueMustNotBeNull);
            }
            User user =new User(model.UserEmail,model.UserPassword);
            var userExist = await _userRepository.Login(user);
            if(userExist is null)
            {
                return new APIResponse<UserViewDTO>(401 , ErrorMessages.NotFound);
            }
            UserViewDTO result = new()
            {
                UserId = userExist.UserId,
                UserEmail = userExist.Email,
                UserPassword = userExist.Password,
                UserRoleName = userExist.Role.RoleName
            };
            return new APIResponse<UserViewDTO>(200 , result);
        }
        catch (Exception ex)
        {
            return new APIResponse<UserViewDTO>(500, ex.Message);
        }
    }
}

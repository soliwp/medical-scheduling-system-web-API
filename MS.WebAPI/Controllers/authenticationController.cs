using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MS.Application.Contracts.authentication;
using MS.Application.Contracts.Helpers;
using MS.Application.Contracts.User;
using MS.Application.Contracts.User.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MS.WebAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class authenticationController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public authenticationController(IUserServices userServices, IConfiguration configuration, ITokenService tokenService)
        {
            _userServices = userServices;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        /// <summary>
        ///     authentication users for using Web API
        ///     after successful authentication, user will get a token in response > headers > authentication
        ///     user for next request must enter token in authorization
        /// </summary>
        /// <param name="model">
        ///     we need email and password for logining
        /// </param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<APIResponse<UserViewDTO>>> Login([FromBody] UserLoginDTO model)
        {
            var user = await _userServices.Login(model);
            if (user.StatusCode != 200)
            {
                return StatusCode(user.StatusCode, user);
            }
            var secret = _configuration.GetValue<string>("secretKey");
            var issuer = _configuration.GetValue<string>("ValidIssuer");
            var audience = _configuration.GetValue<string>("Audience");

            var token = _tokenService.GenerateToken(user.Data, secret , issuer , audience);
            //var token = GenerateToken(user.Data);

            HttpContext.Response.Headers.Add("authentication", token);
            return StatusCode(user.StatusCode, user);
        }
        private string GenerateToken(UserViewDTO user)
        {
            List<Claim> claims = new()
            {
                new Claim("userId" , user.UserId.ToString()),
                new Claim(ClaimTypes.Email , user.UserEmail),
                new Claim(ClaimTypes.Role , user.UserRoleName),
                new Claim("UserPassword" , user.UserPassword),
            };
            var secret = _configuration.GetValue<string>("secretKey");
            var secretkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var Credential = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                    issuer: _configuration.GetValue<string>("ValidIssuer"),
                    audience: _configuration.GetValue<string>("Audience"),
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: Credential
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

using ArdRehber.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace ArdRehber.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected EUserType GetUserType()
        {
            var token = string.Empty;
            var header = (string)HttpContext.Request.Headers["Authorization"];
            if (header != null) { token = header.Substring(7); }

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);

            int userTyp = int.Parse(jwtSecurityToken.Claims.First(claim => claim.Type == "typ").Value);

            return (EUserType)userTyp;

        }

        protected int GetUserId()
        {
            var token = string.Empty;
            var header = (string)HttpContext.Request.Headers["Authorization"];
            if (header != null) { token = header.Substring(7); }

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);

            int userId = int.Parse(jwtSecurityToken.Claims.First(claim => claim.Type == "nameid").Value);

            return userId;

        }

        protected void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}

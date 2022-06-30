#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using ArdRehber.Data;
using ArdRehber.Entities;
using ArdRehber.Dtos;

namespace ArdRehber.Controllers
{

    // POST: api/Logins
    // To protect from overposting attacks, see ttps://go.microsoft.com/fwlink/?linkid=2123754
    //    [HttpPost]
    //    public async Task<ActionResult<User>> PostLogin(LoginDto loginDto )
    //    {

    //         var user=new User()
    //        {
    //            Name = loginDto.UserName,
    //            Password = loginDto.Password,


    //        };


    //        _context.Users.Add(user);
    //        await _context.SaveChangesAsync();

    //        return CreatedAtAction("GetLogin", new { id = user.Id }, user);
    //    }


    //    private bool LoginExists(int id)
    //    {
    //        return _context.Users.Any(e => e.Id == id);
    //    }
    //}




    [ApiController]
    [Route("api/[controller]")]
    public class LoginsController : ControllerBase
    {
        readonly DataContext _context;
        readonly IConfiguration _configuration;
        public LoginsController(DataContext content, IConfiguration configuration)
        {
            _context = content;
            _configuration = configuration;
        }
        
        [HttpPost("action")]
        public async Task<LoginResponseDto> Login([FromBody] LoginDto loginDto)
        {
            User user = await _context.Users.Include(x => x.UserType).FirstOrDefaultAsync(x => x.Email == loginDto.Email);
            if (user != null)
            {
                LoginResponseDto responseDto = new LoginResponseDto();
                if (!VerifyPassword(loginDto.Password, user.PasswordHash, user.PasswordSalt))
                    return null;

                //Token üretiliyor.
                TokenHandler tokenHandler = new TokenHandler(_configuration);
                Token token = tokenHandler.CreateAccessToken(user);

                responseDto.AccessToken = token.AccessToken;
                responseDto.RefreshToken = token.RefreshToken;
                responseDto.Expiration = token.Expiration;
                responseDto.UserDto = new UserDto()
                {
                    Email = user.Email,
                    Name = user.Name,
                    Surname = user.Surname,
                    Id = user.Id,
                    UserTypeName = user.UserType.UserTypeName
                };

                //Refresh token Users tablosuna işleniyor.
                user.RefreshToken = token.RefreshToken;
                user.RefreshTokenEndDate = token.Expiration.AddMinutes(3);
                await _context.SaveChangesAsync();

                return responseDto;


            }
            return null;
        }

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); // Create hash using password salt.
                for (int i = 0; i < computedHash.Length; i++)
                { // Loop through the byte array
                    if (computedHash[i] != passwordHash[i]) return false; // if mismatch
                }
            }
            return true; //if no mismatches.
        }

        [HttpGet("[action]")]
        public async Task<Token> RefreshTokenLogin([FromForm] string refreshToken)
        {
            User user = await _context.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
            if (user != null && user?.RefreshTokenEndDate > DateTime.Now)
            {
                TokenHandler tokenHandler = new TokenHandler(_configuration);
                Token token = tokenHandler.CreateAccessToken(user);

                user.RefreshToken = token.RefreshToken;
                user.RefreshTokenEndDate = token.Expiration.AddMinutes(3);
                await _context.SaveChangesAsync();

                return token;
            }
            return null;
        }



       
    }
}

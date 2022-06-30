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
using System.Security.Claims;
using System.Web.Helpers;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using ArdRehber.FluentValidation;
using FluentValidation.Results;
using System.IdentityModel.Tokens.Jwt;
using ArdRehber.Enums;

namespace ArdRehber.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        private async Task<User> AddUser(UserDto userDto)
        {
                //int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var kontrolUser = await _context.Users.AnyAsync(s => s.Name == userDto.Name && s.Surname == userDto.Surname && s.Email == userDto.Email);

                if (kontrolUser == true)
                {
                    return null;
                }

                var user = new User()
                {
                    Name = userDto.Name,
                    Surname = userDto.Surname,
                    Email = userDto.Email,
                    // Password=userDto.Password,
                    UserTypeId = userDto.UserTypeId

                    //  RefreshToken = "ymMWEVXitFQWmOZmHlIOez6fEnFB5ROIIsasmSCPpT8=",
                    // RefreshTokenEndDate =new DateTime( 2022,5,29)

                };

                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(userDto.Password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                //userDto.Id = user.Id; 

                return user;

            }
            

        private async Task<User> UpdateUser(UserDto userDto)
        {
            var entity = _context.Users.FirstOrDefault(e => e.Id == userDto.Id);


            entity.Name = userDto.Name;
            entity.Surname = userDto.Surname;
            entity.Email = userDto.Email;
            // Password=userDto.Password,
            entity.UserTypeId = userDto.UserTypeId;

                //  RefreshToken = "ymMWEVXitFQWmOZmHlIOez6fEnFB5ROIIsasmSCPpT8=",
                // RefreshTokenEndDate =new DateTime( 2022,5,29)

           

            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }


        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            EUserType userTyp = GetUserType();
            if (userTyp == EUserType.Admin)
            {
                return await _context.Users.ToListAsync();
            }
            else
            {
                return BadRequest("Bu alana erişim yetkiniz bulunmamaktadır.");
            }


        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            EUserType userTyp = GetUserType();
            if (userTyp == EUserType.Admin)
            {
                var user = await _context.Users.FindAsync(id);

                if (user == null)
                {
                    return NotFound();
                }

                return user;
            }
            else
            {
                return BadRequest("Bu alana erişim yetkiniz bulunmamaktadır.");
            }


        }

        [HttpPost("[action]")]

        public async Task<IActionResult> Create([FromForm] UserDto userDto)
        {
            EUserType userTyp = GetUserType();
            if (userTyp == EUserType.Admin)
            {

                UserValidator userValidator = new UserValidator();
                ValidationResult results = userValidator.Validate(userDto);

                if (results.IsValid == false)
                {
                    return BadRequest(results.Errors[0].ErrorMessage);
                }

                if (userDto.Id > 0)
                {
                    var user = this.UpdateUser(userDto).Result;
                    //  personDto.Id = person.Id;

                }
                else
                {
                    var user = this.AddUser(userDto).Result;
                    if (user == null)
                    {
                        return BadRequest("Aynı kişiyi tekrar ekleyemezsiniz.");
                    }

                    userDto.Id = user.Id;
                }


                // else => ekleme

                return Ok(userDto);

            }
            else
            {
                return BadRequest("Bu alana erişim yetkiniz bulunmamaktadır.");
            }
        }
       

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            EUserType userTyp = GetUserType();
            if (userTyp == EUserType.Admin)
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            else
            {
                return BadRequest("Bu alana erişim yetkiniz bulunmamaktadır.");
            }

        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }


    }
}

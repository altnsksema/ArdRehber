using ArdRehber.Data;
using ArdRehber.Dtos;
using ArdRehber.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArdRehber.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerifyPassword : BaseController
    {
        private readonly DataContext _context;
        string passPhrase = "abc123";

        public VerifyPassword(DataContext context)
        {
            _context = context;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromForm] ChangeForgatPasswordDto changeForgatPasswordDto)
        {

            string sifreCoz = StringCipher.Decrypt(changeForgatPasswordDto.SifrelnemsiVeri, passPhrase);

            SifreDegistirmeInfo yeniInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<SifreDegistirmeInfo>(sifreCoz);


            var kontrolUser = await _context.Users.FirstOrDefaultAsync(s => s.Id== yeniInfo.UserId);

            if (kontrolUser == null)
            {
                return NotFound();
            }

          

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(changeForgatPasswordDto.Password, out passwordHash, out passwordSalt);

            kontrolUser.PasswordHash = passwordHash;
            kontrolUser.PasswordSalt = passwordSalt;

            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}

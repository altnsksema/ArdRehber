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
    public  class ForgatPassword : ControllerBase
    {
        private readonly DataContext _context;
        string passPhrase = "abc123";

        public ForgatPassword(DataContext context)
        {
            _context = context;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromBody] ForgatPasswordDto forgatPasswordDto)
        {
            var kontrolUser = await _context.Users.FirstOrDefaultAsync(s => s.Email == forgatPasswordDto.UserName);

            if (kontrolUser == null)
            {
                return NotFound();
            }

            SifreDegistirmeInfo info = new SifreDegistirmeInfo();

            info.UserId = kontrolUser.Id;
            info.Tarih = DateTime.Now;

            string objeStr = Newtonsoft.Json.JsonConvert.SerializeObject(info);

            string sifrelenmisObje = StringCipher.Encrypt(objeStr, passPhrase);

            return Ok(sifrelenmisObje);
        }
    }
}

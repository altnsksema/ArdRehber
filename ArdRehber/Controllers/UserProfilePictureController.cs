using ArdRehber.Data;
using ArdRehber.Dtos;
using ArdRehber.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArdRehber.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserProfilePictureController : BaseController
    {
        private readonly DataContext _context;

        public UserProfilePictureController(DataContext context)
        {
            _context = context;
        }

        private async Task<Image> AddImage(ImageDto model)
        {
            if (model.ImageBase64 != null)
            {
                return null;
            }
            Image img = new Image();
            img.UserId = GetUserId();
            img.ImageBase64 = Convert.FromBase64String(model.ImageBase64);

            _context.Images.Add(img);
            await _context.SaveChangesAsync();
            return img;

        }

        private async Task<Image> UpdateImage(ImageDto model)
        {
            var entity = _context.Images.FirstOrDefault(e => e.Id == model.Id);

            entity.UserId = GetUserId();
            entity.ImageBase64 = Convert.FromBase64String(model.ImageBase64); 
           

            _context.Images.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<Image>>> GetPersons()
        {
            return await _context.Images.Include(x => x.User).ToListAsync();//Include(x=>x.Department)    İNCLUDE İŞLEMİ sss  Include(x => x.Department).
        }

        // GET: api/Persons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Image>> GetImage(int id)
        {
            var image = await _context.Images.FindAsync(id);

            if (image == null)
            {
                return NotFound();
            }

            return image;
        }


        [HttpPost("ImagePost")]
        public async Task<IActionResult> Create([FromBody] ImageDto model)
        {
            //byte[] bytes = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAApgAAAKYB3X3/OAAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAANCSURBVEiJtZZPbBtFFMZ/M7ubXdtdb1xSFyeilBapySVU8h8OoFaooFSqiihIVIpQBKci6KEg9Q6H9kovIHoCIVQJJCKE1ENFjnAgcaSGC6rEnxBwA04Tx43t2FnvDAfjkNibxgHxnWb2e/u992bee7tCa00YFsffekFY+nUzFtjW0LrvjRXrCDIAaPLlW0nHL0SsZtVoaF98mLrx3pdhOqLtYPHChahZcYYO7KvPFxvRl5XPp1sN3adWiD1ZAqD6XYK1b/dvE5IWryTt2udLFedwc1+9kLp+vbbpoDh+6TklxBeAi9TL0taeWpdmZzQDry0AcO+jQ12RyohqqoYoo8RDwJrU+qXkjWtfi8Xxt58BdQuwQs9qC/afLwCw8tnQbqYAPsgxE1S6F3EAIXux2oQFKm0ihMsOF71dHYx+f3NND68ghCu1YIoePPQN1pGRABkJ6Bus96CutRZMydTl+TvuiRW1m3n0eDl0vRPcEysqdXn+jsQPsrHMquGeXEaY4Yk4wxWcY5V/9scqOMOVUFthatyTy8QyqwZ+kDURKoMWxNKr2EeqVKcTNOajqKoBgOE28U4tdQl5p5bwCw7BWquaZSzAPlwjlithJtp3pTImSqQRrb2Z8PHGigD4RZuNX6JYj6wj7O4TFLbCO/Mn/m8R+h6rYSUb3ekokRY6f/YukArN979jcW+V/S8g0eT/N3VN3kTqWbQ428m9/8k0P/1aIhF36PccEl6EhOcAUCrXKZXXWS3XKd2vc/TRBG9O5ELC17MmWubD2nKhUKZa26Ba2+D3P+4/MNCFwg59oWVeYhkzgN/JDR8deKBoD7Y+ljEjGZ0sosXVTvbc6RHirr2reNy1OXd6pJsQ+gqjk8VWFYmHrwBzW/n+uMPFiRwHB2I7ih8ciHFxIkd/3Omk5tCDV1t+2nNu5sxxpDFNx+huNhVT3/zMDz8usXC3ddaHBj1GHj/As08fwTS7Kt1HBTmyN29vdwAw+/wbwLVOJ3uAD1wi/dUH7Qei66PfyuRj4Ik9is+hglfbkbfR3cnZm7chlUWLdwmprtCohX4HUtlOcQjLYCu+fzGJH2QRKvP3UNz8bWk1qMxjGTOMThZ3kvgLI5AzFfo379UAAAAASUVORK5CYII=");

            //Image img = new Image();
            //img.UserId = GetUserId();
            //img.ImageBase64 = Convert.FromBase64String(model.ImageBase64);

            //_context.Images.Add(img);
            //await _context.SaveChangesAsync();
            //return Ok();

            if (model.Id > 0)
            {
                var img = this.UpdateImage(model).Result;
                //  personDto.Id = person.Id;

            }
            else
            {
                var img = this.AddImage(model).Result;
                if (img == null)
                {
                    return BadRequest("Aynı kullanıcıya birden fazla fotoğraf ekleyemezsiniz.");
                }

                model.Id = img.Id;
            }


            // else => ekleme

            return Ok(model);


        }

        // DELETE: api/Persons/5

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
                var img = await _context.Images.FindAsync(id);
                if (img == null)
                {
                    return NotFound();
                }

                _context.Images.Remove(img);
                await _context.SaveChangesAsync();

                return NoContent();
            
        }

        private bool ImageExists(int id)
        {
            return _context.Images.Any(e => e.Id == id);
        }
    }
}


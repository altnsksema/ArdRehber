using ArdRehber.Data;
using ArdRehber.Dtos;
using ArdRehber.Entities;
using ArdRehber.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArdRehber.Controllers
{   [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TitlesController : BaseController
    {
        private readonly DataContext _context;

        public TitlesController(DataContext context)
        {
            _context = context;
        }

        private async Task<Title> AddTitle(TitleDto titleDto)
        {
            var kontrolTitle = await _context.Titles.AnyAsync(s => s.TitleName == titleDto.TitleName );

            if (kontrolTitle == true)
            {
                return null;
            }

            var title = new Title()
            {
                TitleName = titleDto.TitleName,
                Order= titleDto.Order

            };

            _context.Titles.Add(title);
            await _context.SaveChangesAsync();
            return title;
        }

        private async Task<Title> UpdateTitle(TitleDto titleDto)
        {
            var entity = _context.Titles.FirstOrDefault(e => e.Id == titleDto.Id);

            entity.Id = titleDto.Id;
            entity.TitleName = titleDto.TitleName;
            entity.Order = titleDto.Order;

            


            _context.Titles.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Title>>> GetTitle()
        {
            return await _context.Titles.ToListAsync();
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Title>> GetTitle(int id)
        {
            var title = await _context.Titles.FindAsync(id);

            if (title == null)
            {
                return NotFound();
            }

            return title;
        }

        [HttpPost]
        public async Task<ActionResult<Person>> PostTitle(TitleDto titleDto)
        {
            EUserType userTyp = GetUserType();
            if (userTyp == EUserType.Admin)
            {

                if (titleDto.Id > 0)
                {
                    var title = this.UpdateTitle(titleDto).Result;

                    var departmentTitles = _context.TitleDepartments.Where(s => s.TitleId == title.Id);

                    if (departmentTitles!= null && departmentTitles.Count()> 0)
                    {
                        _context.TitleDepartments.RemoveRange(departmentTitles);
                    }

                    foreach (var departmentId in titleDto.DepartmentIdlist)
                    {
                        TitleDepartment titleDepartment = new TitleDepartment();
                        titleDepartment.TitleId = title.Id;
                        titleDepartment.DepartmentId = departmentId;

                        _context.TitleDepartments.Add(titleDepartment);
                        await _context.SaveChangesAsync();

                    }
                }
                else
                {
                    var title = this.AddTitle(titleDto).Result;


                    if (title == null)
                    {
                        return BadRequest("Aynı unvanı tekrar ekleyemezsiniz.");
                    }


                    if (titleDto.DepartmentIdlist != null && titleDto.DepartmentIdlist.Count>0)
                    {
                        foreach (var departmentId in titleDto.DepartmentIdlist  )
                        {
                            TitleDepartment titleDepartment = new TitleDepartment();
                            titleDepartment.TitleId = title.Id;
                            titleDepartment.DepartmentId = departmentId;

                            _context.TitleDepartments.Add(titleDepartment);
                            await _context.SaveChangesAsync();
                        }
                    }

                    titleDto.Id = title.Id;
                }

                return Ok(titleDto);

            }
            else
            {
                return BadRequest("Bu alana erişim yetkiniz bulunmamaktadır.");
            }
        }

      
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTitle(int id)
        {
            EUserType userTyp = GetUserType();
            if (userTyp == EUserType.Admin)
            {

                var title = await _context.Titles.FindAsync(id);
                if (title == null)
                {
                    return NotFound();
                }

                _context.Titles.Remove(title);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            else
            {
                return BadRequest("Bu alana erişim yetkiniz bulunmamaktadır.");
            }

        }

        private bool PersonExists(int id)
        {
            return _context.Titles.Any(e => e.Id == id);
        }

    }
}

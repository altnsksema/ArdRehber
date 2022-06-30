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
using Microsoft.AspNetCore.Authorization;
using ArdRehber.FluentValidation;
using FluentValidation.Results;
using ArdRehber.Enums;

namespace ArdRehber.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : BaseController
    {
        private readonly DataContext _context;

        public DepartmentsController(DataContext context)
        {
            _context = context;
        }

        private async Task<Department> AddDepartment(DepartmentDto departmentDto)
        {
            var kontrolDepartment = await _context.Departments.AnyAsync(s => s.DepartmentName == departmentDto.DepartmentName);

            if (kontrolDepartment == true)
            {
                return null;
            }

            var department = new Department()
            {
                
                DepartmentName = departmentDto.DepartmentName,
               
            };


            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return department;
        }

        private async Task<Department> UpdateDepartment(DepartmentDto departmentDto)
        {
            var entity = _context.Departments.FirstOrDefault(e => e.DepartmentId == departmentDto.DepartmentId);

            entity.DepartmentId = departmentDto.DepartmentId;
            entity.DepartmentName = departmentDto.DepartmentName;
           
            _context.Departments.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        [HttpGet("GetPersonDepartmentById/{id}")]
        public async Task<ActionResult<List<Person>>> PersonDepartment(int id)
        {
            var personList =  await _context.Persons.Where(e => e.DepartmentId == id).Include(x=>x.Title).Include(x => x.Department).OrderBy(x=>x.Title.Order).ToListAsync();

                                                                                                           
            return personList;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
                return await _context.Departments.ToListAsync(); 
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            
                var department = await _context.Departments.FindAsync(id);

                if (department == null)
                {
                    return NotFound();
                }

                return department;
            

        }

        [HttpPost]
        public async Task<ActionResult<Department>> PostDepartment(DepartmentDto departmentDto)
        {
            EUserType userTyp = GetUserType();
            if (userTyp == EUserType.Admin)
            {
                DepartmentValidator departmentValidator = new DepartmentValidator();
                ValidationResult results = departmentValidator.Validate(departmentDto);

                if (results.IsValid == false)
                {
                    return BadRequest(results.Errors[0].ErrorMessage);
                }

                if (departmentDto.DepartmentId > 0)
                {
                    var department = this.UpdateDepartment(departmentDto).Result;

                }
                else
                {
                    var department = this.AddDepartment(departmentDto).Result;
                    if (department == null)
                    {
                        return BadRequest("Böyle bir departman zaten var.");
                    }

                    departmentDto.DepartmentId = department.DepartmentId;
                }

                return Ok(departmentDto);



                //_context.Departments.Add(department);
                //await _context.SaveChangesAsync();
                //return CreatedAtAction("GetDepartment", new { id = department.DepartmentId }, department);
            }
            else
            {
                return BadRequest("Bu alana erişim yetkiniz bulunmamaktadır.");
            }

        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            EUserType userTyp = GetUserType();
            if (userTyp == EUserType.Admin)
            {
                var department = await _context.Departments.FindAsync(id);
                if (department == null)
                {
                    return NotFound();
                }

                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            else
            {
                return BadRequest("Bu alana erişim yetkiniz bulunmamaktadır.");
            }

        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.DepartmentId == id);
        }


        [HttpGet("TitleByDepartmentId/{id}")]
        public async Task<ActionResult<List<Title>>> TitleByDepartmentId(int id)
        {
            var titles = await _context.TitleDepartments
                .Where(e => e.DepartmentId == id)
                .Include(x => x.Title)
                .Select(x=>x.Title)
                .OrderBy(x => x.Order).ToListAsync();
            return titles;
        }
    }
}

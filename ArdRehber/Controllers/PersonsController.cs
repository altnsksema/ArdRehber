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
using ArdRehber.FluentValidation;
using FluentValidation.Results;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using ArdRehber.Enums;

namespace ArdRehber.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : BaseController
    {
        private readonly DataContext _context;

        public PersonsController(DataContext context)
        {
            _context = context;
        }

        private async Task<Person> AddPerson(PersonDto personDto)
        {
            var kontrolPerson = await _context.Persons.AnyAsync(s => s.Name == personDto.Name && s.SurName == personDto.SurName && s.PhoneNumber == personDto.PhoneNumber
         && s.InternalNumber == personDto.InternalNumber && s.DepartmentId == personDto.DepartmentId);

            if (kontrolPerson == true)
            {
                return null;
            }

            var person = new Person()
            {
                Name = personDto.Name,
                SurName = personDto.SurName,
                PhoneNumber = personDto.PhoneNumber,
                InternalNumber = personDto.InternalNumber,
                DepartmentId = personDto.DepartmentId,
                TitleId=personDto.TitleId
              

            };


            _context.Persons.Add(person);
            await _context.SaveChangesAsync();
            return person;
        }

        private async Task<Person> UpdatePerson(PersonDto personDto)
        {
            var entity = _context.Persons.FirstOrDefault(e => e.Id == personDto.Id);

            entity.Id = personDto.Id;
            entity.Name = personDto.Name;
            entity.SurName = personDto.SurName;
            entity.PhoneNumber = personDto.PhoneNumber;
            entity.InternalNumber = personDto.InternalNumber;
            entity.DepartmentId = personDto.DepartmentId;
            entity.TitleId = personDto.TitleId;
            
            
            _context.Persons.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }


        // GET: api/Persons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
        {
            return await _context.Persons.Include(x => x.Department).Include(x=>x.Title).ToListAsync();//Include(x=>x.Department) İNCLUDE İŞLEMİ sss  Include(x => x.Department).
            
        }


        // GET: api/Persons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            var person = await _context.Persons.FindAsync(id);

            if (person == null)
            {
                 return NotFound();
            }

            return person;
        }

        

        // POST: api/Persons
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
       
        
	
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(PersonDto personDto)
        {
            EUserType userTyp = GetUserType();
            if (userTyp == EUserType.Admin)
            {
                PersonValidator personValidator = new PersonValidator();
                ValidationResult results = personValidator.Validate(personDto);

                if (results.IsValid == false)
                {
                    return BadRequest(results.Errors[0].ErrorMessage);
                }


                if (personDto.Id > 0)
                {
                    var person = this.UpdatePerson(personDto).Result;
                      personDto.Id = person.Id;

                }
                else
                {
                    var person = this.AddPerson(personDto).Result;
                    if (person == null)
                    {
                        return BadRequest("Aynı kişiyi tekrar ekleyemezsiniz.");
                    }

                    personDto.Id = person.Id;
                }


                // else => ekleme

                return Ok(personDto);

                //return CreatedAtAction("GetPerson", new { id = person.Id }, person);

                //PersonValidator personValidator=new PersonValidator();
                //ValidationResult results = personValidator.Validate(person);
                //personValidator.Validate(person);
                //
            }
            else
            {
                return BadRequest("Bu alana erişim yetkiniz bulunmamaktadır.");
            }
        }

        // DELETE: api/Persons/5
       
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            EUserType userTyp = GetUserType();
            if (userTyp == EUserType.Admin)
            {

                var person = await _context.Persons.FindAsync(id);
                if (person == null)
                {
                    return NotFound();
                }

                _context.Persons.Remove(person);
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
            return _context.Persons.Any(e => e.Id == id);
        }
    }
}

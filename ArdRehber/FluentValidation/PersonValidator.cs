using ArdRehber.Dtos;
using ArdRehber.Entities;
using FluentValidation;

namespace ArdRehber.FluentValidation
{

    public class PersonValidator: AbstractValidator<PersonDto>
    {
        public PersonValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Lütfen Adı Giriniz");
            RuleFor(x => x.SurName).NotEmpty().WithMessage("Lütfen Soyadı Giriniz");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Telefon Numarası Alanı Zorunludur");
            RuleFor(x => x.DepartmentId).NotEmpty().WithMessage("Departman Alanı Zorunludur");
            RuleFor(x => x.Name).MaximumLength(50).WithMessage("Lütfen Ad alanına 50 karakterden fazla değer girişi yapmayın.");
            RuleFor(x => x.SurName).MaximumLength(50).WithMessage("Lütfen Soyad alanına 50 karakterden fazla değer girişi yapmayın.");
            // RuleFor(x => x.PhoneNumber).MinimumLength(3).WithMessage("Lütfen en az 1 karakter giriniz ");
        }
    }
}

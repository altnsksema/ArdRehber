using ArdRehber.Dtos;
using FluentValidation;

namespace ArdRehber.FluentValidation
{
    public class UserValidator: AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Lütfen Adı Giriniz");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Lütfen Soyadı Giriniz");
            RuleFor(x => x.Email).NotEmpty().WithMessage("E-mail Alanı Zorunludur");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Şifre Alanı Zorunludur");
            RuleFor(x => x.Name).MaximumLength(50).WithMessage("Lütfen Ad alanına 50 karakterden fazla değer girişi yapmayın.");
            RuleFor(x => x.Surname).MaximumLength(50).WithMessage("Lütfen Soyad alanına 50 karakterden fazla değer girişi yapmayın.");
            // RuleFor(x => x.PhoneNumber).MinimumLength(3).WithMessage("Lütfen en az 1 karakter giriniz ");

        }
    }
}

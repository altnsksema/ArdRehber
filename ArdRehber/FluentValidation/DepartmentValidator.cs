using ArdRehber.Dtos;
using FluentValidation;

namespace ArdRehber.FluentValidation
{
    public class DepartmentValidator : AbstractValidator<DepartmentDto>
    {
        public DepartmentValidator()
        {
            RuleFor(x => x.DepartmentName).NotEmpty().WithMessage("Lütfen Departman Adı Giriniz");
           

            RuleFor(x => x.DepartmentName).MaximumLength(50).WithMessage("Lütfen  Departman Adı alanına 50 karakterden fazla değer girişi yapmayın.");
            
        }
    }
}

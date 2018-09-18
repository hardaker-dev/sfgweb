using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaasFeeGuides.ViewModels.Validation
{
    public class RegistrationViewModelValidator : AbstractValidator<CustomerAccount>
    {
        public RegistrationViewModelValidator()
        {
            RuleFor(vm => vm.Email).NotEmpty().WithMessage("Email cannot be empty");
            RuleFor(vm => vm.Password).NotEmpty().WithMessage("Password cannot be empty");
            RuleFor(vm => vm.FirstName).NotEmpty().WithMessage("FirstName cannot be empty");
            RuleFor(vm => vm.LastName).NotEmpty().WithMessage("LastName cannot be empty");
            RuleFor(vm => vm.DateOfBirth).NotEmpty().WithMessage("DataOfBirth cannot be min value");
            RuleFor(vm => vm.Username).NotEmpty().WithMessage("Username cannot be empty");
        }
    }
}

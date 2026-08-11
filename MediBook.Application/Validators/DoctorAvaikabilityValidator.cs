using FluentValidation;
using MediBook.Application.Dtos;

namespace MediBook.Application.Validators
{
    public class DoctorAvailabilityValidator : AbstractValidator<DoctorAvailabilityDto>
    {
        public DoctorAvailabilityValidator()
        {
            RuleFor(x => x.DoctorId)
                .GreaterThan(0).WithMessage("A valid DoctorId is required.");

            RuleFor(x => x.EndTime)
                .GreaterThan(x => x.StartTime)
                .WithMessage("EndTime must be after StartTime.");
        }
    }
}
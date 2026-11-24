using FluentValidation;

namespace Heapify.Application.Projects.Commands.ReserveNextNumber;

internal sealed class ReserveNextNumberValidator : AbstractValidator<ReserveNextNumberRequest>
{
    public ReserveNextNumberValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty();
    }
}
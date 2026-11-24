using FluentValidation;

namespace Heapify.Application.Projects.Commands.CreateProject;

internal sealed class CreateProjectValidator : AbstractValidator<CreateProjectRequest>
{
    public CreateProjectValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);
    }
}
using FluentValidation;

namespace Heapify.Application.Projects.DTO;

public sealed class CreateProjectDto
{
    public required string Name { get; init; }
}

internal sealed class CreateProjectDtoValidator : AbstractValidator<CreateProjectDto>
{
    public CreateProjectDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);
    }
}
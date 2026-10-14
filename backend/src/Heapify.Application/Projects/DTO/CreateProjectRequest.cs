using FluentValidation;

namespace Heapify.Application.Projects.DTO;

public sealed class CreateProjectRequest
{
    public required string Name { get; init; }
}

internal sealed class CreateProjectRequestValidator : AbstractValidator<CreateProjectRequest>
{
    public CreateProjectRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);
    }
}
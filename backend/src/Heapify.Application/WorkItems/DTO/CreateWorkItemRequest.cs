using FluentValidation;

namespace Heapify.Application.WorkItems.DTO;

public sealed class CreateWorkItemRequest
{
    public required Guid ProjectId { get; init; }
    public required string Title { get; init; }
}

internal sealed class CreateWorkItemRequestValidator : AbstractValidator<CreateWorkItemRequest>
{
    public CreateWorkItemRequestValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(100);
    }
}
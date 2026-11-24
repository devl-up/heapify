using FluentValidation;

namespace Heapify.Application.WorkItems.Commands.CreateWorkItem;

internal sealed class CreateWorkItemValidator : AbstractValidator<CreateWorkItemRequest>
{
    public CreateWorkItemValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(100);
    }
}
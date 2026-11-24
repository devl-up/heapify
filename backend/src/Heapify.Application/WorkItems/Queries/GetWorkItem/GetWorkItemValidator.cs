using FluentValidation;

namespace Heapify.Application.WorkItems.Queries.GetWorkItem;

internal sealed class GetWorkItemValidator : AbstractValidator<GetWorkItemRequest>
{
    public GetWorkItemValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty();

        RuleFor(x => x.Number)
            .GreaterThan(0);
    }
}
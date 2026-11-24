using FluentValidation;
using Heapify.Application.Common.Extensions;

namespace Heapify.Application.WorkItems.Queries.GetWorkItems;

internal sealed class GetWorkItemsValidator : AbstractValidator<GetWorkItemsRequest>
{
    public GetWorkItemsValidator()
    {
        RuleFor(x => x.Index)
            .ValidatePageIndex();

        RuleFor(x => x.Size)
            .ValidatePageSize();

        RuleFor(x => x.ProjectId)
            .NotEmpty();
    }
}
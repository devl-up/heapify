using FluentValidation;
using Heapify.Application.Common.Extensions;

namespace Heapify.Application.WorkItems.DTO;

public sealed class GetWorkItemsRequest
{
    public required int Index { get; init; }
    public required int Size { get; init; }
    public required Guid ProjectId { get; init; }
}

internal sealed class GetWorkItemsRequestValidator : AbstractValidator<GetWorkItemsRequest>
{
    public GetWorkItemsRequestValidator()
    {
        RuleFor(x => x.Index)
            .ValidatePageIndex();

        RuleFor(x => x.Size)
            .ValidatePageSize();

        RuleFor(x => x.ProjectId)
            .NotEmpty();
    }
}
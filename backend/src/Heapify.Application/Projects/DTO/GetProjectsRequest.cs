using FluentValidation;
using Heapify.Application.Common.Extensions;

namespace Heapify.Application.Projects.DTO;

public sealed class GetProjectsRequest
{
    public required int Index { get; init; }
    public required int Size { get; init; }
}

internal sealed class GetProjectsRequestValidator : AbstractValidator<GetProjectsRequest>
{
    public GetProjectsRequestValidator()
    {
        RuleFor(x => x.Index)
            .ValidatePageIndex();

        RuleFor(x => x.Size)
            .ValidatePageSize();
    }
}
using FluentValidation;
using Heapify.Application.Common.Extensions;

namespace Heapify.Application.Projects.Queries.GetProjects;

internal sealed class GetProjectsValidator : AbstractValidator<GetProjectsRequest>
{
    public GetProjectsValidator()
    {
        RuleFor(x => x.Index)
            .ValidatePageIndex();

        RuleFor(x => x.Size)
            .ValidatePageSize();
    }
}
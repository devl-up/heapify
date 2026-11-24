using FluentValidation;

namespace Heapify.Application.Projects.Queries.GetProject;

internal sealed class GetProjectValidator : AbstractValidator<GetProjectRequest>
{
    public GetProjectValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
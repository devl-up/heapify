using FluentValidation;
using Heapify.Application.Common.Data;
using Heapify.Application.Common.Requests;
using Heapify.Domain.Projects.Entities;
using Microsoft.EntityFrameworkCore;

namespace Heapify.Application.Projects.Queries.GetProject;

public sealed class GetProjectHandler(IApplicationDbContext dbContext, IValidator<GetProjectRequest> validator)
    : IRequestHandler<GetProjectRequest, GetProjectResponse>
{
    public async Task<GetProjectResponse> HandleAsync(GetProjectRequest request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        return await dbContext.Set<Project>()
            .Where(p => p.Id == request.Id)
            .Select(p => new GetProjectResponse
            {
                Id = p.Id,
                Name = p.Name
            })
            .FirstAsync(cancellationToken);
    }
}
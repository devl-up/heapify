using FluentValidation;
using Heapify.Application.Common.Data;
using Heapify.Application.Common.DTO;
using Heapify.Application.Common.Requests;
using Heapify.Application.Users.Services;
using Heapify.Domain.Projects.Entities;
using Microsoft.EntityFrameworkCore;

namespace Heapify.Application.Projects.Queries.GetProjects;

public sealed class GetProjectsHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUserService,
    IValidator<GetProjectsRequest> validator) : IRequestHandler<GetProjectsRequest, PaginatedResponse<GetProjectsResponse>>
{
    public async Task<PaginatedResponse<GetProjectsResponse>> HandleAsync(GetProjectsRequest request,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var currentUser = currentUserService.Get();

        var query = dbContext.Set<Project>()
            .Where(p => p.Members.Any(m => m.UserId == currentUser.Id));

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(p => p.Name)
            .Skip(request.Index * request.Size)
            .Take(request.Size)
            .Select(p => new GetProjectsResponse
            {
                Id = p.Id,
                Name = p.Name
            })
            .ToListAsync(cancellationToken);

        return new PaginatedResponse<GetProjectsResponse>
        {
            Items = items,
            TotalCount = totalCount
        };
    }
}
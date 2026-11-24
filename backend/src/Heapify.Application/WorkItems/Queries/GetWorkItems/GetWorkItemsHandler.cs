using FluentValidation;
using Heapify.Application.Common.Data;
using Heapify.Application.Common.DTO;
using Heapify.Application.Common.Requests;
using Heapify.Domain.WorkItems.Entities;
using Microsoft.EntityFrameworkCore;

namespace Heapify.Application.WorkItems.Queries.GetWorkItems;

public sealed class GetWorkItemsHandler(IApplicationDbContext dbContext, IValidator<GetWorkItemsRequest> validator)
    : IRequestHandler<GetWorkItemsRequest, PaginatedResponse<GetWorkItemsResponse>>
{
    public async Task<PaginatedResponse<GetWorkItemsResponse>> HandleAsync(GetWorkItemsRequest request,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var query = dbContext.Set<WorkItem>()
            .Where(w => w.ProjectId == request.ProjectId);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(w => w.Number)
            .Skip(request.Index * request.Size)
            .Take(request.Size)
            .Select(w => new GetWorkItemsResponse
            {
                Number = w.Number,
                Title = w.Title
            })
            .ToListAsync(cancellationToken);

        return new PaginatedResponse<GetWorkItemsResponse>
        {
            TotalCount = totalCount,
            Items = items
        };
    }
}
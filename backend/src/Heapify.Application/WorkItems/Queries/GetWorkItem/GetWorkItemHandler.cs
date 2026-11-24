using FluentValidation;
using Heapify.Application.Common.Data;
using Heapify.Application.Common.Requests;
using Heapify.Domain.WorkItems.Entities;
using Microsoft.EntityFrameworkCore;

namespace Heapify.Application.WorkItems.Queries.GetWorkItem;

public sealed class GetWorkItemHandler(IApplicationDbContext dbContext, IValidator<GetWorkItemRequest> validator)
    : IRequestHandler<GetWorkItemRequest, GetWorkItemResponse>
{
    public async Task<GetWorkItemResponse> HandleAsync(GetWorkItemRequest request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        return await dbContext.Set<WorkItem>()
            .Where(w => w.ProjectId == request.ProjectId && w.Number == request.Number)
            .Select(w => new GetWorkItemResponse
            {
                Number = w.Number,
                Title = w.Title
            })
            .FirstAsync(cancellationToken);
    }
}
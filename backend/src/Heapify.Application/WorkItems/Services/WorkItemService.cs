using FluentValidation;
using Heapify.Application.Common.Data;
using Heapify.Application.Common.DTO;
using Heapify.Application.Projects.Services;
using Heapify.Application.WorkItems.DTO;
using Heapify.Domain.WorkItems.Entities;
using Microsoft.EntityFrameworkCore;

namespace Heapify.Application.WorkItems.Services;

internal sealed class WorkItemService(
    IApplicationDbContext dbContext,
    IProjectService projectService,
    IValidator<CreateWorkItemRequest> createWorkItemValidator,
    IValidator<GetWorkItemsRequest> getWorkItemsValidator) : IWorkItemService
{
    public async Task<long> CreateAsync(CreateWorkItemRequest request, CancellationToken cancellationToken)
    {
        await createWorkItemValidator.ValidateAndThrowAsync(request, cancellationToken);

        await using var transaction = await dbContext.BeginTransactionAsync();

        var number = await projectService.ReserveNextNumberAsync(request.ProjectId, cancellationToken);
        var workItem = new WorkItem
        {
            ProjectId = request.ProjectId,
            Number = number,
            Title = request.Title
        };

        await dbContext.Set<WorkItem>().AddAsync(workItem, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return workItem.Number;
    }

    public async Task<PaginatedResponse<WorkItemResponse>> GetAllAsync(GetWorkItemsRequest request,
        CancellationToken cancellationToken)
    {
        await getWorkItemsValidator.ValidateAndThrowAsync(request, cancellationToken);

        var query = dbContext.Set<WorkItem>()
            .Where(w => w.ProjectId == request.ProjectId);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(w => w.Number)
            .Skip(request.Index * request.Size)
            .Take(request.Size)
            .Select(w => new WorkItemResponse
            {
                Number = w.Number,
                Title = w.Title
            })
            .ToListAsync(cancellationToken);

        return new PaginatedResponse<WorkItemResponse>
        {
            TotalCount = totalCount,
            Items = items
        };
    }
}
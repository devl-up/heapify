using FluentValidation;
using Heapify.Application.Common.Data;
using Heapify.Application.Projects.Services;
using Heapify.Application.WorkItems.DTO;
using Heapify.Domain.WorkItems.Entities;

namespace Heapify.Application.WorkItems.Services;

internal sealed class WorkItemService(
    IApplicationDbContext dbContext,
    IProjectService projectService,
    IValidator<CreateWorkItemRequest> createWorkItemValidator) : IWorkItemService
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
}
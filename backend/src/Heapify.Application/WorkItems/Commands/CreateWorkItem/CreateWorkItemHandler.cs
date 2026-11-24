using FluentValidation;
using Heapify.Application.Common.Data;
using Heapify.Application.Common.Requests;
using Heapify.Application.Projects.Commands.ReserveNextNumber;
using Heapify.Domain.WorkItems.Entities;

namespace Heapify.Application.WorkItems.Commands.CreateWorkItem;

public sealed class CreateWorkItemHandler(
    IApplicationDbContext dbContext,
    IValidator<CreateWorkItemRequest> validator,
    ReserveNextNumberHandler reserveNextNumberHandler) : IRequestHandler<CreateWorkItemRequest, long>
{
    public async Task<long> HandleAsync(CreateWorkItemRequest request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        await using var transaction = await dbContext.BeginTransactionAsync();

        var reserveNextNumberCommand = new ReserveNextNumberRequest
        {
            ProjectId = request.ProjectId
        };

        var number = await reserveNextNumberHandler.HandleAsync(reserveNextNumberCommand, cancellationToken);

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
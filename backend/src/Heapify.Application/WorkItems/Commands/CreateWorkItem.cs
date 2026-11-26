using FluentValidation;
using Heapify.Application.Common.Data;
using Heapify.Application.Common.Requests;
using Heapify.Application.Projects.Commands;
using Heapify.Domain.WorkItems.Entities;

namespace Heapify.Application.WorkItems.Commands;

public static class CreateWorkItem
{
    public sealed record Request
    {
        public required Guid ProjectId { get; init; }
        public required string Title { get; init; }
    }

    public sealed class Handler(
        IApplicationDbContext dbContext,
        IValidator<Request> validator,
        ReserveNextNumber.Handler reserveNextNumberHandler) : IRequestHandler<Request, long>
    {
        public async Task<long> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);

            await using var transaction = await dbContext.BeginTransactionAsync();

            var reserveNextNumberRequest = new ReserveNextNumber.Request
            {
                ProjectId = request.ProjectId
            };

            var number = await reserveNextNumberHandler.HandleAsync(reserveNextNumberRequest, cancellationToken);

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

    internal sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}
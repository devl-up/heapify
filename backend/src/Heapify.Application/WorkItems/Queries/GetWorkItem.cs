using FluentValidation;
using Heapify.Application.Common.Data;
using Heapify.Application.Common.Requests;
using Heapify.Domain.WorkItems.Entities;
using Microsoft.EntityFrameworkCore;

namespace Heapify.Application.WorkItems.Queries;

public static class GetWorkItem
{
    public sealed record Request
    {
        public required Guid ProjectId { get; init; }
        public required long Number { get; init; }
    }

    public sealed record Response
    {
        public required long Number { get; init; }
        public required string Title { get; init; }
    }

    public sealed class Handler(
        IApplicationDbContext dbContext,
        IValidator<Request> validator)
        : IRequestHandler<Request, Response>
    {
        public async Task<Response> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);

            return await dbContext.Set<WorkItem>()
                .Where(w => w.ProjectId == request.ProjectId && w.Number == request.Number)
                .Select(w => new Response
                {
                    Number = w.Number,
                    Title = w.Title
                })
                .FirstAsync(cancellationToken);
        }
    }

    internal sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty();

            RuleFor(x => x.Number)
                .GreaterThan(0);
        }
    }
}
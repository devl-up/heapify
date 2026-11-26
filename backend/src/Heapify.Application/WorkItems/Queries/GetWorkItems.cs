using FluentValidation;
using Heapify.Application.Common.Data;
using Heapify.Application.Common.DTO;
using Heapify.Application.Common.Extensions;
using Heapify.Application.Common.Requests;
using Heapify.Domain.WorkItems.Entities;
using Microsoft.EntityFrameworkCore;

namespace Heapify.Application.WorkItems.Queries;

public static class GetWorkItems
{
    public sealed record Request
    {
        public required int Index { get; init; }
        public required int Size { get; init; }
        public required Guid ProjectId { get; init; }
    }

    public sealed record Response
    {
        public required long Number { get; init; }
        public required string Title { get; init; }
    }

    public sealed class Handler(
        IApplicationDbContext dbContext,
        IValidator<Request> validator)
        : IRequestHandler<Request, PaginatedResponse<Response>>
    {
        public async Task<PaginatedResponse<Response>> HandleAsync(Request request,
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
                .Select(w => new Response
                {
                    Number = w.Number,
                    Title = w.Title
                })
                .ToListAsync(cancellationToken);

            return new PaginatedResponse<Response>
            {
                TotalCount = totalCount,
                Items = items
            };
        }
    }

    internal sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Index)
                .ValidatePageIndex();

            RuleFor(x => x.Size)
                .ValidatePageSize();

            RuleFor(x => x.ProjectId)
                .NotEmpty();
        }
    }
}
using FluentValidation;
using Heapify.Application.Common.Data;
using Heapify.Application.Common.DTO;
using Heapify.Application.Common.Extensions;
using Heapify.Application.Common.Requests;
using Heapify.Application.Users.Services;
using Heapify.Domain.Projects.Entities;
using Microsoft.EntityFrameworkCore;

namespace Heapify.Application.Projects.Queries;

public static class GetProjects
{
    public sealed record Request
    {
        public required int Index { get; init; }
        public required int Size { get; init; }
    }

    public sealed record Response
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
    }

    public sealed class Handler(
        IApplicationDbContext dbContext,
        ICurrentUserService currentUserService,
        IValidator<Request> validator) : IRequestHandler<Request, PaginatedResponse<Response>>
    {
        public async Task<PaginatedResponse<Response>> HandleAsync(Request request,
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
                .Select(p => new Response
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToListAsync(cancellationToken);

            return new PaginatedResponse<Response>
            {
                Items = items,
                TotalCount = totalCount
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
        }
    }
}
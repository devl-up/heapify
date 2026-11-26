using FluentValidation;
using Heapify.Application.Common.Data;
using Heapify.Application.Common.Requests;
using Heapify.Domain.Projects.Entities;
using Microsoft.EntityFrameworkCore;

namespace Heapify.Application.Projects.Queries;

public static class GetProject
{
    public sealed record Request
    {
        public required Guid Id { get; init; }
    }

    public sealed record Response
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
    }

    public sealed class Handler(
        IApplicationDbContext dbContext,
        IValidator<Request> validator)
        : IRequestHandler<Request, Response>
    {
        public async Task<Response> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);

            return await dbContext.Set<Project>()
                .Where(p => p.Id == request.Id)
                .Select(p => new Response
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .FirstAsync(cancellationToken);
        }
    }

    internal sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }
}
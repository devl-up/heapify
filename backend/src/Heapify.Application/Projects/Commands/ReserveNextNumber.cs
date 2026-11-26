using FluentValidation;
using Heapify.Application.Common.Data;
using Heapify.Application.Common.Requests;
using Microsoft.EntityFrameworkCore;

namespace Heapify.Application.Projects.Commands;

public static class ReserveNextNumber
{
    public sealed record Request
    {
        public required Guid ProjectId { get; init; }
    }

    public sealed class Handler(
        IApplicationDbContext dbContext,
        IValidator<Request> validator)
        : IRequestHandler<Request, long>
    {
        public async Task<long> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);

            var numbers = await dbContext
                .SqlQuery<long>($"""
                                 UPDATE project
                                 SET next_number = next_number + 1
                                 WHERE id = {request.ProjectId}
                                 RETURNING next_number - 1
                                 """)
                .ToListAsync(cancellationToken);

            return numbers.Single();
        }
    }

    internal sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty();
        }
    }
}
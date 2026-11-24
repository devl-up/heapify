using FluentValidation;
using Heapify.Application.Common.Data;
using Heapify.Application.Common.Requests;
using Microsoft.EntityFrameworkCore;

namespace Heapify.Application.Projects.Commands.ReserveNextNumber;

public sealed class ReserveNextNumberHandler(
    IApplicationDbContext dbContext,
    IValidator<ReserveNextNumberRequest> validator)
    : IRequestHandler<ReserveNextNumberRequest, long>
{
    public async Task<long> HandleAsync(ReserveNextNumberRequest request, CancellationToken cancellationToken)
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
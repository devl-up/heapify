using FluentValidation;
using Heapify.Application.Common.Data;
using Heapify.Application.Common.Requests;
using Heapify.Application.Users.Services;
using Heapify.Domain.Projects.Entities;
using Heapify.Domain.Projects.Enums;

namespace Heapify.Application.Projects.Commands;

public static class CreateProject
{
    public sealed record Request
    {
        public required string Name { get; init; }
    }

    public sealed class Handler(
        IApplicationDbContext dbContext,
        ICurrentUserService currentUserService,
        IValidator<Request> validator) : IRequestHandler<Request, Guid>
    {
        public async Task<Guid> HandleAsync(Request request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);

            var currentUser = currentUserService.Get();

            var project = new Project
            {
                Id = Guid.CreateVersion7(),
                Name = request.Name,
                NextNumber = 1
            };

            var member = new Member
            {
                ProjectId = project.Id,
                UserId = currentUser.Id,
                Role = MemberRole.Admin
            };

            await dbContext.Set<Project>().AddAsync(project, cancellationToken);
            await dbContext.Set<Member>().AddAsync(member, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return project.Id;
        }
    }

    internal sealed class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
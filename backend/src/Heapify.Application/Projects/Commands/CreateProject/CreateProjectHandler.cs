using FluentValidation;
using Heapify.Application.Common.Data;
using Heapify.Application.Common.Requests;
using Heapify.Application.Users.Services;
using Heapify.Domain.Projects.Entities;
using Heapify.Domain.Projects.Enums;

namespace Heapify.Application.Projects.Commands.CreateProject;

public sealed class CreateProjectHandler(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUserService,
    IValidator<CreateProjectRequest> validator) : IRequestHandler<CreateProjectRequest, Guid>
{
    public async Task<Guid> HandleAsync(CreateProjectRequest request, CancellationToken cancellationToken)
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
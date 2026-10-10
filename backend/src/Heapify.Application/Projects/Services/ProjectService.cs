using FluentValidation;
using Heapify.Application.Common.Data;
using Heapify.Application.Projects.DTO;
using Heapify.Application.Users.Services;
using Heapify.Domain.Projects.Entities;
using Heapify.Domain.Projects.Enums;

namespace Heapify.Application.Projects.Services;

internal sealed class ProjectService(
    IApplicationDbContext dbContext,
    ICurrentUserService currentUserService,
    IValidator<CreateProjectDto> createProjectValidator) : IProjectService
{
    public async Task<Guid> CreateAsync(CreateProjectDto dto, CancellationToken cancellationToken = default)
    {
        await createProjectValidator.ValidateAndThrowAsync(dto, cancellationToken);

        var currentUser = await currentUserService.GetAsync(cancellationToken);

        var project = new Project
        {
            Id = Guid.CreateVersion7(),
            Name = dto.Name
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
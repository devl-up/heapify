using FluentValidation;
using Heapify.Application.Common.Data;
using Heapify.Application.Common.DTO;
using Heapify.Application.Projects.DTO;
using Heapify.Application.Users.Services;
using Heapify.Domain.Projects.Entities;
using Heapify.Domain.Projects.Enums;
using Microsoft.EntityFrameworkCore;

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

    public async Task<PaginatedResult<ProjectDto>> GetAllAsync(int index, int size,
        CancellationToken cancellationToken = default)
    {
        var currentUser = await currentUserService.GetAsync(cancellationToken);

        var query = dbContext.Set<Project>()
            .Where(p => p.Members.Any(m => m.UserId == currentUser.Id));

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(p => p.Name)
            .Skip(index * size)
            .Take(size)
            .Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name
            })
            .ToListAsync(cancellationToken);

        return new PaginatedResult<ProjectDto>
        {
            Items = items,
            TotalCount = totalCount
        };
    }
}
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
    IValidator<CreateProjectRequest> createProjectValidator,
    IValidator<GetProjectsRequest> getProjectsValidator) : IProjectService
{
    public async Task<Guid> CreateAsync(CreateProjectRequest request, CancellationToken cancellationToken)
    {
        await createProjectValidator.ValidateAndThrowAsync(request, cancellationToken);

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

    public async Task<PaginatedResponse<ProjectResponse>> GetAllAsync(GetProjectsRequest request,
        CancellationToken cancellationToken)
    {
        await getProjectsValidator.ValidateAndThrowAsync(request, cancellationToken);

        var currentUser = currentUserService.Get();

        var query = dbContext.Set<Project>()
            .Where(p => p.Members.Any(m => m.UserId == currentUser.Id));

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(p => p.Name)
            .Skip(request.Index * request.Size)
            .Take(request.Size)
            .Select(p => new ProjectResponse
            {
                Id = p.Id,
                Name = p.Name
            })
            .ToListAsync(cancellationToken);

        return new PaginatedResponse<ProjectResponse>
        {
            Items = items,
            TotalCount = totalCount
        };
    }

    public async Task<ProjectResponse> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.Set<Project>()
            .Where(p => p.Id == id)
            .Select(p => new ProjectResponse
            {
                Id = p.Id,
                Name = p.Name
            })
            .FirstAsync(cancellationToken);
    }

    public async Task<long> ReserveNextNumberAsync(Guid id, CancellationToken cancellationToken)
    {
        var numbers = await dbContext
            .SqlQuery<int>($"""
                            UPDATE project
                            SET next_number = next_number + 1
                            WHERE id = {id}
                            RETURNING next_number - 1
                            """)
            .ToListAsync(cancellationToken);

        return numbers.Single();
    }
}
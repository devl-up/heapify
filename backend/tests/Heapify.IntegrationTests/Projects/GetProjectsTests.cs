using Heapify.Application.Common.DTO;
using Heapify.Application.Projects.DTO;
using Heapify.Domain.Projects.Entities;
using Heapify.Domain.Projects.Enums;
using Heapify.Domain.Users.Entities;
using Heapify.IntegrationTests.Common;
using Heapify.IntegrationTests.Common.Extensions;
using Heapify.IntegrationTests.Common.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace Heapify.IntegrationTests.Projects;

public sealed class GetProjectsTests(IntegrationTestFixture fixture)
    : IntegrationTestBase(fixture)
{
    [Fact]
    public async Task Get_Projects_ReturnsOk_WhenAuthenticated()
    {
        // Arrange
        using var client = await CreateClient()
            .AuthenticateAsync();

        var projects = new List<Project>
        {
            new()
            {
                Id = Guid.CreateVersion7(),
                Name = "Project 1"
            },
            new()
            {
                Id = Guid.CreateVersion7(),
                Name = "Project 2"
            }
        };

        await using (var dbContext = Database.CreateContext())
        {
            var user = await dbContext.Set<User>().FirstAsync();

            var members = projects.Select(p => new Member
            {
                ProjectId = p.Id,
                UserId = user.Id,
                Role = MemberRole.Admin
            });

            await dbContext.Set<Project>().AddRangeAsync(projects);
            await dbContext.Set<Member>().AddRangeAsync(members);
            await dbContext.SaveChangesAsync();
        }

        // Act
        var response = await client.GetFromJsonAsync<PaginatedResponse<ProjectResponse>>("/api/v1/projects?index=0&size=10");

        // Assert
        Assert.NotNull(response);

        var expectedTotalCount = projects.Count;
        Assert.Equal(expectedTotalCount, response.TotalCount);

        var expectedProjects = projects.Select(p => new ProjectResponse
        {
            Id = p.Id,
            Name = p.Name
        });

        Assert.Equivalent(expectedProjects, response.Items);
    }
}
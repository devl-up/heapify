using System.Net;
using Heapify.Application.Projects.DTO;
using Heapify.Domain.Projects.Entities;
using Heapify.Domain.Projects.Enums;
using Heapify.IntegrationTests.Common;
using Heapify.IntegrationTests.Common.Extensions;
using Heapify.IntegrationTests.Common.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace Heapify.IntegrationTests.Projects;

public class CreateProjectTests(IntegrationTestFixture fixture)
    : IntegrationTestBase(fixture)
{
    [Fact]
    public async Task Post_CreateProject_ReturnsCreated_WhenValidRequest()
    {
        // Arrange
        using var client = await CreateClient()
            .AuthenticateAsync();

        var dto = new CreateProjectDto
        {
            Name = "Test Project"
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/projects", dto);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var locationId = response.Headers.Location?.OriginalString.Split('/').Last();
        Assert.NotNull(locationId);
        Assert.True(Guid.TryParse(locationId, out var id));

        var expectedProject = new Project
        {
            Id = id,
            Name = dto.Name
        };

        await using var context = Database.CreateContext();

        var project = await context.Set<Project>()
            .AsNoTracking()
            .FirstAsync(p => p.Id == id);

        Assert.Equivalent(expectedProject, project);

        var members = await context.Set<Member>()
            .Where(m => m.ProjectId == id)
            .Select(m => m.Role)
            .ToListAsync();

        Assert.Single(members);
        Assert.Equal(MemberRole.Admin, members[0]);
    }
}
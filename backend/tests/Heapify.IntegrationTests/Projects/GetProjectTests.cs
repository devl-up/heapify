using Heapify.Application.Projects.DTO;
using Heapify.Domain.Projects.Entities;
using Heapify.Domain.Projects.Enums;
using Heapify.Domain.Users.Entities;
using Heapify.IntegrationTests.Common;
using Heapify.IntegrationTests.Common.Extensions;
using Heapify.IntegrationTests.Common.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace Heapify.IntegrationTests.Projects;

public sealed class GetProjectTests(IntegrationTestFixture fixture) : IntegrationTestBase(fixture)
{
    [Fact]
    public async Task Get_Project_ReturnsOk_WhenAuthenticated()
    {
        // Arrange
        using var client = await CreateClient()
            .AuthenticateAsync();

        var project = new Project
        {
            Id = Guid.CreateVersion7(),
            Name = "Project 1",
            NextNumber = 1
        };

        await using (var dbContext = Database.CreateContext())
        {
            var user = await dbContext.Set<User>().FirstAsync();

            var member = new Member
            {
                ProjectId = project.Id,
                UserId = user.Id,
                Role = MemberRole.Viewer
            };

            await dbContext.Set<Project>().AddAsync(project);
            await dbContext.Set<Member>().AddAsync(member);
            await dbContext.SaveChangesAsync();
        }

        // Act
        var response = await client.GetFromJsonAsync<ProjectResponse>($"/api/v1/projects/{project.Id}");

        // Assert
        Assert.NotNull(response);

        var expectedProject = new ProjectResponse
        {
            Id = project.Id,
            Name = project.Name
        };

        Assert.Equivalent(expectedProject, response);
    }
    
    [Fact]
    public async Task Get_Project_ReturnsForbidden_WhenNotViewer()
    {
        // Arrange
        using var client = await CreateClient()
            .AuthenticateAsync();

        var project = new Project
        {
            Id = Guid.CreateVersion7(),
            Name = "Project 1",
            NextNumber = 1
        };

        await using (var dbContext = Database.CreateContext())
        {
            await dbContext.Set<Project>().AddAsync(project);
            await dbContext.SaveChangesAsync();
        }

        // Act
        var response = await client.GetAsync($"/api/v1/projects/{project.Id}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Forbidden, response.StatusCode);
    }
}
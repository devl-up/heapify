using System.Net;
using Heapify.Application.WorkItems.Queries.GetWorkItem;
using Heapify.Domain.Projects.Entities;
using Heapify.Domain.Projects.Enums;
using Heapify.Domain.Users.Entities;
using Heapify.Domain.WorkItems.Entities;
using Heapify.IntegrationTests.Common;
using Heapify.IntegrationTests.Common.Extensions;
using Heapify.IntegrationTests.Common.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace Heapify.IntegrationTests.WorkItems;

public sealed class GetWorkItemTests(IntegrationTestFixture fixture) : IntegrationTestBase(fixture)
{
    [Fact]
    public async Task Get_WorkItem_ReturnsOk_WhenAuthenticated()
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

        var workItems = new List<WorkItem>
        {
            new()
            {
                ProjectId = project.Id,
                Number = 1,
                Title = "Work Item 1"
            },
            new()
            {
                ProjectId = project.Id,
                Number = 2,
                Title = "Work Item 2"
            }
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
            await dbContext.Set<WorkItem>().AddRangeAsync(workItems);
            await dbContext.SaveChangesAsync();
        }

        // Act
        var response =
            await client.GetFromJsonAsync<GetWorkItemResponse>(
                $"/api/v1/projects/{project.Id}/work-items/{workItems.First().Number}");

        // Assert
        Assert.NotNull(response);

        var expectedWorkItem = workItems
            .OrderBy(wi => wi.Number)
            .Select(wi => new GetWorkItemResponse
            {
                Number = wi.Number,
                Title = wi.Title
            })
            .First();

        Assert.Equivalent(expectedWorkItem, response);
    }

    [Fact]
    public async Task Get_WorkItem_ReturnsForbidden_WhenNotViewer()
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
        var response = await client.GetAsync($"/api/v1/projects/{project.Id}/work-items/1");

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}
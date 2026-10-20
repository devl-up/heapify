using System.Net;
using Heapify.Application.WorkItems.DTO;
using Heapify.Domain.Projects.Entities;
using Heapify.Domain.Projects.Enums;
using Heapify.Domain.Users.Entities;
using Heapify.Domain.WorkItems.Entities;
using Heapify.IntegrationTests.Common;
using Heapify.IntegrationTests.Common.Extensions;
using Heapify.IntegrationTests.Common.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace Heapify.IntegrationTests.WorkItems;

public sealed class CreateWorkItemTests(IntegrationTestFixture fixture)
    : IntegrationTestBase(fixture)
{
    [Fact]
    public async Task Post_CreateWorkItem_ReturnsCreated_WhenRequestIsValid()
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
                Role = MemberRole.Member
            };

            await dbContext.Set<Project>().AddAsync(project);
            await dbContext.Set<Member>().AddAsync(member);
            await dbContext.SaveChangesAsync();
        }

        var request = new CreateWorkItemRequest
        {
            ProjectId = project.Id,
            Title = "Test Work Item"
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/work-items", request);

        // Assert
        response.EnsureSuccessStatusCode();
        var locationId = response.Headers.Location?.OriginalString.Split('/').Last();
        Assert.NotNull(locationId);
        Assert.True(int.TryParse(locationId, out var number));

        var expectedWorkItem = new WorkItem
        {
            ProjectId = project.Id,
            Number = number,
            Title = request.Title
        };

        await using var context = Database.CreateContext();

        var workItem = await context.Set<WorkItem>()
            .AsNoTracking()
            .FirstAsync(w => w.Number == number && w.ProjectId == project.Id);

        Assert.Equivalent(expectedWorkItem, workItem);

        var updatedProject = await context.Set<Project>()
            .AsNoTracking()
            .FirstAsync(p => p.Id == project.Id);

        var expectedNumber = project.NextNumber + 1;

        Assert.Equal(expectedNumber, updatedProject.NextNumber);
    }

    [Fact]
    public async Task Post_CreateWorkItem_ReturnsForbidden_WhenNotMember()
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

        var request = new CreateWorkItemRequest
        {
            ProjectId = project.Id,
            Title = "Test Work Item"
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/v1/work-items", request);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}
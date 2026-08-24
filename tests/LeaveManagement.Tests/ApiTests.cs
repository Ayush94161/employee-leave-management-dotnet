using Xunit;
using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace LeaveManagement.Tests;

public class ApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    public ApiTests(WebApplicationFactory<Program> factory) => _client = factory.CreateClient();

    [Fact]
    public async Task Health_ReturnsOk() => Assert.Equal(HttpStatusCode.OK, (await _client.GetAsync("/health")).StatusCode);

    [Fact]
    public async Task Employees_ReturnsOk() => Assert.Equal(HttpStatusCode.OK, (await _client.GetAsync("/api/employees")).StatusCode);
}

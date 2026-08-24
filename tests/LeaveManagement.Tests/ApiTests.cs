using Xunit;
using System.Net;

namespace LeaveManagement.Tests;

public class ApiTests : IClassFixture<TestApiFactory>
{
    private readonly HttpClient _client;
    public ApiTests(TestApiFactory factory) => _client = factory.CreateClient();

    [Fact]
    public async Task Health_ReturnsOk() => Assert.Equal(HttpStatusCode.OK, (await _client.GetAsync("/health")).StatusCode);

    [Fact]
    public async Task Employees_ReturnsOk() => Assert.Equal(HttpStatusCode.OK, (await _client.GetAsync("/api/employees")).StatusCode);
}

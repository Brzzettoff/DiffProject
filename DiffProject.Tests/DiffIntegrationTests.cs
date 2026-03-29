using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

namespace DiffProject.Tests;

public class DiffIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public DiffIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Should_Return404_When_DataMissing()
    {
        var id = "failed-test-noData";
        var response = await _client.GetAsync($"/v1/diff/{id}");

        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);

    }

    [Fact]
    public async Task Should_Return40U_When_UploadInvalidBase64()
    {
        // Arrange
        var id = "fail-test-not64";
        var badPayload = new { data = "!!! Not Base64 !!!" };

        // Act
        var response = await _client.PutAsJsonAsync($"/v1/diff/{id}/left", badPayload);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);

        var errorBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("Not a B64 data", errorBody);
    }

    [Fact]
    public async Task Should_ReturnEquals_When_DataMatches()
    {
        var id = "integration-test-allGood";
        var payload = new { data = "AAAAAA==" };

        // 1. Act: Upload Left
        var leftRes = await _client.PutAsJsonAsync($"/v1/diff/{id}/left", payload);
        Assert.True(leftRes.IsSuccessStatusCode);

        // 2. Act: Upload Right
        var rightRes = await _client.PutAsJsonAsync($"/v1/diff/{id}/right", payload);
        Assert.True(rightRes.IsSuccessStatusCode);

        // 3. Act: Get the Diff Result
        var result = await _client.GetFromJsonAsync<dynamic>($"/v1/diff/{id}");

        // 4. Assert
        string type = result?.GetProperty("diffResultType").GetString();
        Assert.Equal("Equals", type);
    }

    //TODO: add tests
}
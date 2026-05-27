using FluentAssertions;
using Sensor.API.Tests.Infrastructure;

namespace Sensor.API.Tests.GraphQL;

[Collection(ApiIntegrationCollection.Name)]
public sealed class RoomQueryTests : IAsyncLifetime
{
    private readonly ApiFixture _fixture;
    private HttpClient _client = null!;

    public RoomQueryTests(ApiFixture fixture)
    {
        _fixture = fixture;
    }

    public Task InitializeAsync()
    {
        _client = _fixture.Factory.CreateClient();
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await DatabaseSeeder.ClearRoomsAsync(_fixture.Factory.Services);
        _client.Dispose();
    }

    [Fact]
    public async Task GetRooms_returns_empty_list_when_database_is_empty()
    {
        await DatabaseSeeder.ClearRoomsAsync(_fixture.Factory.Services);

        var response = await GraphQlTestClient.PostAsync(
            _client,
            """
            query {
              rooms {
                id
                name
              }
            }
            """);

        var data = await GraphQlTestClient.ReadDataAsync(response);
        data.GetProperty("rooms").GetArrayLength().Should().Be(0);
    }

    [Fact]
    public async Task GetRooms_returns_seeded_rooms()
    {
        await DatabaseSeeder.ClearRoomsAsync(_fixture.Factory.Services);
        await DatabaseSeeder.SeedRoomAsync(_fixture.Factory.Services, "Conference A");
        await DatabaseSeeder.SeedRoomAsync(_fixture.Factory.Services, "Lab B");

        var response = await GraphQlTestClient.PostAsync(
            _client,
            """
            query {
              rooms(page: 1, pageSize: 10) {
                id
                name
              }
            }
            """);

        var data = await GraphQlTestClient.ReadDataAsync(response);
        var rooms = data.GetProperty("rooms");
        rooms.GetArrayLength().Should().Be(2);

        var names = rooms.EnumerateArray()
            .Select(r => r.GetProperty("name").GetString())
            .ToList();
        names.Should().BeEquivalentTo(["Conference A", "Lab B"]);
    }

    [Fact]
    public async Task GetRoomById_returns_room_when_it_exists()
    {
        await DatabaseSeeder.ClearRoomsAsync(_fixture.Factory.Services);
        var room = await DatabaseSeeder.SeedRoomAsync(
            _fixture.Factory.Services,
            "Server Room");

        var response = await GraphQlTestClient.PostAsync(
            _client,
            """
            query ($id: UUID!) {
              roomById(id: $id) {
                id
                name
              }
            }
            """,
            new { id = room.Id });

        var data = await GraphQlTestClient.ReadDataAsync(response);
        var result = data.GetProperty("roomById");
        Guid.Parse(result.GetProperty("id").GetString()!).Should().Be(room.Id);
        result.GetProperty("name").GetString().Should().Be("Server Room");
    }
}

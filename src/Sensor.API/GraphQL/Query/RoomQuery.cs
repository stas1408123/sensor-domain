using AutoMapper;
using Sensor.API.ViewModels;
using Sensor.BLL.Models;
using Sensor.BLL.Services.Abstaction;

namespace Sensor.API.GraphQL.Query
{
    public class RoomQuery
    {
        private readonly IGenericService<Room> _roomService;
        private readonly IMapper _mapper;
        private readonly ILogger<RoomQuery> _logger;

        public RoomQuery(IGenericService<Room> roomService, IMapper mapper, ILogger<RoomQuery> logger)
        {
            _roomService = roomService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IReadOnlyList<RoomViewModel>> GetRooms()
        {
            _logger.LogInformation("GraphQL query GetRooms requested");
            var rooms = await _roomService.GetAll();
            _logger.LogInformation("GraphQL query GetRooms returned {Count} rooms", rooms.Count);
            return _mapper.Map<IReadOnlyList<RoomViewModel>>(rooms);
        }

        public async Task<RoomViewModel> GetRoomById(Guid id)
        {
            _logger.LogInformation("GraphQL query GetRoomById requested for id {RoomId}", id);
            var room = await _roomService.GetById(id);
            return _mapper.Map<RoomViewModel>(room);
        }
    }
}

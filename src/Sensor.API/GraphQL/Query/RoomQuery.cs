using AutoMapper;
using Sensor.API.ViewModels;
using Sensor.BLL.Services.Abstaction;

namespace Sensor.API.GraphQL.Query
{
    public class RoomQuery
    {
        private readonly IRoomService _roomService;
        private readonly IMapper _mapper;
        private readonly ILogger<RoomQuery> _logger;

        public RoomQuery(IRoomService roomService, IMapper mapper, ILogger<RoomQuery> logger)
        {
            _roomService = roomService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IReadOnlyList<RoomViewModel>> GetRooms(
            DateTime? from = null,
            DateTime? to = null,
            int page = 1,
            int pageSize = 20)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 200);

            _logger.LogInformation(
                "GraphQL query GetRooms requested. from={From} to={To} page={Page} pageSize={PageSize}",
                from,
                to,
                page,
                pageSize);

            var pagedRooms = await _roomService.GetPagedWithDateFilter(from, to, page, pageSize);

            _logger.LogInformation(
                "GraphQL query GetRooms returned {Count} rooms",
                pagedRooms.Count);

            return _mapper.Map<IReadOnlyList<RoomViewModel>>(pagedRooms);
        }

        public async Task<RoomViewModel> GetRoomById(Guid id)
        {
            _logger.LogInformation("GraphQL query GetRoomById requested for id {RoomId}", id);
            var room = await _roomService.GetById(id);
            return _mapper.Map<RoomViewModel>(room);
        }
    }
}

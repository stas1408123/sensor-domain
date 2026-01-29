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

        public RoomQuery(IGenericService<Room> roomService, IMapper mapper)
        {
            _roomService = roomService;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<RoomViewModel>> GetRooms()
        {
            return _mapper.Map<IReadOnlyList<RoomViewModel>>(await _roomService.GetAll());
        }

        public async Task<RoomViewModel> GetRoomById(Guid id)
        {
            return _mapper.Map<RoomViewModel>(await _roomService.GetById(id));
        }
    }
}

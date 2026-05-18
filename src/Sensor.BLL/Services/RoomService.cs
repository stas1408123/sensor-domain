using AutoMapper;
using Sensor.BLL.Models;
using Sensor.BLL.Services.Abstaction;
using Sensor.DAL.Entities;
using Sensor.DAL.Repositories.Abstarction;

namespace Sensor.BLL.Services
{
    public class RoomService : GenericService<Room, RoomEntity>, IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public RoomService(
            IGenericRepository<RoomEntity> repository,
            IRoomRepository roomRepository,
            IMapper mapper) : base(repository, mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<Room>> GetPagedWithDateFilter(
            DateTime? from,
            DateTime? to,
            int page,
            int pageSize)
        {
            var rooms = await _roomRepository.GetPagedWithDateFilter(from, to, page, pageSize);
            return _mapper.Map<IReadOnlyList<Room>>(rooms);
        }
    }
}

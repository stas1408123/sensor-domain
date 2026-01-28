using AutoMapper;
using Sensor.BLL.Services.Abstaction;
using Sensor.DAL.Repositories.Abstarction;
using System.Linq.Expressions;

namespace Sensor.BLL.Services
{
    public class GenericService<TModel, TEntity> : IGenericService<TModel>
        where TEntity : class
        where TModel : class
    {
        private readonly IGenericRepository<TEntity> _repository;
        private readonly IMapper _mapper;

        public GenericService(IGenericRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TModel> Add(TModel model)
        {
            return _mapper.Map<TModel>(await _repository.Add(_mapper.Map<TEntity>(model)));
        }

        public async Task Delete(Guid id)
        {
            await _repository.Delete(id);
        }

        public async Task<IReadOnlyList<TModel>> GetAll()
        {
            return _mapper.Map<IReadOnlyList<TModel>>(await _repository.GetAll());
        }

        public async Task<TModel> GetById(Guid id)
        {
            return _mapper.Map<TModel>(await _repository.GetById(id));
        }

        public async Task<IEnumerable<TModel>> Get(Expression<Func<TModel, bool>> predicate)
        {
            var result = _mapper.Map<IEnumerable<TModel>>(
                await _repository.Get(_mapper.Map<Expression<Func<TEntity, bool>>>(predicate)));

            return result;
        }

        public async Task<TModel> Update(TModel model)
        {
            return _mapper.Map<TModel>(await _repository.Update(_mapper.Map<TEntity>(model)));
        }
    }
}

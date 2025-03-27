using AutoMapper;
using PureLifeClinic.Application.Interfaces.IMapper;

namespace PureLifeClinic.Application.Mapper
{
    public class BaseMapper<TSource, TDestination> : IBaseMapper<TSource, TDestination>
    {
        private readonly IMapper _mapper;

        public BaseMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDestination MapModel(TSource source)
        {
            return _mapper.Map<TDestination>(source);
        }

        public TDestination MapModel(TSource source, TDestination destination)
        {
            return _mapper.Map(source, destination);
        }

        public IEnumerable<TDestination> MapList(IEnumerable<TSource> source)
        {
            return _mapper.Map<IEnumerable<TDestination>>(source);
        }
    }
}

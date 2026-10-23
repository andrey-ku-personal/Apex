namespace Apex.Portfolio.Factories;

public interface IMapperFactory
{
    TMapper GetMapper<TMapper>()
        where TMapper : class;
}

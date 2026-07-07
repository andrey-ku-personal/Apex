namespace Apex.Invest.Factories;

public interface IMapperFactory
{
    TMapper GetMapper<TMapper>()
        where TMapper : class;
}

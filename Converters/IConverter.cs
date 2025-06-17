namespace TalentFlow.Demo.Converters
{
    public interface IConverter<TSource, TDestination>
    {
        TDestination Convert(TSource source);
        TDestination? ConvertNull(TSource? source);
        List<TDestination> Convert(List<TSource> sourceList);
    }
}


using AutoMapper;

/// <summary>
public class GenericListResolver<TSource, USource> : IValueResolver<TSource, USource, List<int>>
{
    private readonly Func<TSource, string> _sourcePropertySelector;

    public GenericListResolver(Func<TSource, string> sourcePropertySelector)
    {
        _sourcePropertySelector = sourcePropertySelector;
    }
    public List<int>? Resolve(TSource source, USource destination, List<int> destMember, ResolutionContext context)
    {

        var sourceValue = _sourcePropertySelector(source);
        return !string.IsNullOrEmpty(sourceValue)
            ? sourceValue.Split(',').Select(int.Parse).ToList()
            : null; // new List<int>();
    }
}
public class GenericListReverseResolver<USource, TSource> : IValueResolver<USource, TSource, string>
{
    private readonly Func<USource, List<int>> _sourcePropertySelector;

    public GenericListReverseResolver(Func<USource, List<int>> sourcePropertySelector)
    {
        _sourcePropertySelector = sourcePropertySelector;
    }

    public string Resolve(USource source, TSource destination, string destMember, ResolutionContext context)
    {
        var sourceValue = _sourcePropertySelector(source);
        return sourceValue != null && sourceValue.Any()
            ? string.Join(",", sourceValue)
            : string.Empty;
    }
}


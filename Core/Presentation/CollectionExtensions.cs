using System.Collections.ObjectModel;

namespace AppFinalAv4A.Core.Presentation;

public static class CollectionExtensions
{

    public static void ReplaceWith<T>(this ObservableCollection<T> target, IEnumerable<T> items)
    {
        target.Clear();
        foreach (var item in items)
            target.Add(item);
    }
}

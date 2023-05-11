using System.Collections;
using System.Windows.Controls;

namespace LenovoYogaToolkit.WPF.Extensions;

public static class ItemCollectionExtensions
{
    public static void AddRange(this ItemCollection itemCollection, IEnumerable enumerable)
    {
        foreach (var item in enumerable)
            itemCollection.Add(item);
    }
}
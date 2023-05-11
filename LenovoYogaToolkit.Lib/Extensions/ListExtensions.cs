﻿using System.Collections;

namespace LenovoYogaToolkit.Lib.Extensions;

public static class ListExtensions
{

    public static object[] ToArray(this IList source)
    {
        var array = new object[source.Count];
        source.CopyTo(array, 0);
        return array;
    }
}
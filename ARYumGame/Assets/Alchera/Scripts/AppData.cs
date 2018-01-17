// ---------------------------------------------------------------------------
//
// Copyright (c) 2018 Alchera, Inc. - All rights reserved.
//
// This example script is under BSD-3-Clause licence.
//
// Author
//       Park DongHa     | dh.park@alcherainc.com
//
// ---------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Set of configuration values to reduce macro in behaviour scripts
/// </summary>
public static class AppData
{
    static IDictionary Table = new Dictionary<int, object>();

    public static void Set<InType>(InType thing)
    {
        var key = typeof(InType).GetHashCode();
        if (Table.Contains(key) == false)
            Table.Add(key, thing);
        else
            Table[key] = thing;
    }

    public static InType Get<InType>()
    {
        var key = typeof(InType).GetHashCode();
        if (Table.Contains(key) == false)
            return default(InType);
        else
            return (InType)Table[key];
    }

}

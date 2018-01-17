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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Alchera
{
    public static class Marker
    {
        public static void Points(Texture2D tex2d, Vector2[] points, Color color)
        {
            foreach (var point in points)
            {
                Point(tex2d, point, color);
            }
        }

        public static void Point(Texture2D tex2d, Vector2 point, Color color)
        {
            int x = (int)point.x;
            int y = (int)point.y;

            tex2d.SetPixel(x + 2, y + 1, color);
            tex2d.SetPixel(x + 1, y + 1, color);
            tex2d.SetPixel(x + 0, y + 1, color);
            tex2d.SetPixel(x - 1, y + 1, color);
            tex2d.SetPixel(x - 2, y + 1, color);

            tex2d.SetPixel(x + 2, y + 0, color);
            tex2d.SetPixel(x + 1, y + 0, color);
            tex2d.SetPixel(x + 0, y + 0, color);
            tex2d.SetPixel(x - 1, y + 0, color);
            tex2d.SetPixel(x - 2, y + 0, color);

            tex2d.SetPixel(x + 2, y - 1, color);
            tex2d.SetPixel(x + 1, y - 1, color);
            tex2d.SetPixel(x + 0, y - 1, color);
            tex2d.SetPixel(x - 1, y - 1, color);
            tex2d.SetPixel(x - 2, y - 1, color);
        }
    }
}

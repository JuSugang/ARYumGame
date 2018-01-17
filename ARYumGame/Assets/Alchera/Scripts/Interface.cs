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
using UnityEngine;

namespace Alchera
{
    public interface IFrameProcessor
    {
        void OnFrame(Alchera.FrameData frame, uint degree);
    }
}

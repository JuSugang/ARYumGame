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
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace Alchera{
    
    public class ConvertTest
    {
        Texture2D input;

        [SetUp]
        public void PrepareTexture()
        {
            input = Resources.Load("UVMap") as Texture2D;
            Assert.NotNull(input);
            Debug.Log(input.format);
            Debug.Log(input.width);
            Debug.Log(input.height);
        }

        [Test]
        public void NoResult()
        {
            var frame = FrameData.Process(input);
            Assert.NotNull(frame);
        }

    }


}

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

namespace Alchera
{
    public class ModuleTest
    {

        [Test]
        public void CheckVersion()
        {
            var version = Alchera.Module.Version;
            Assert.NotNull(version);
            Assert.NotZero(version.Length);
            Debug.Log(version);
        }

        [Test]
        public void CheckFaceDetector()
        {
            IFaceDetector detector = Alchera.Module.Face();
            Assert.NotNull(detector);

            Debug.Log(detector.Version);
        }



    }

}

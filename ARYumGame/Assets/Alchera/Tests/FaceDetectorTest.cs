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
using System;
using System.Collections;

namespace Alchera
{
    public class FaceDetectorTest
    {
        IFaceDetector detector = null;
       
        [SetUp]
        public void CreateDetector(){
            detector = Alchera.Module.Face();
            Assert.NotNull(detector);
        }

        [TearDown]
        public void DisposeDetector(){
            if(detector != null)
                detector.Dispose();
        }

        [Test]
        public void CheckVersion()
        {
            var version = detector.Version;
            Assert.NotNull(version);
            Assert.NotZero(version.Length);
            Debug.Log(version);
        }

        [Test]
        public void ChangeSmoothingFilter()
        {
            detector.Smoothing = 0.65f;
            Assert.AreEqual(0.65f, detector.Smoothing);

            detector.Smoothing = 0.75f;
            Assert.AreEqual(0.75f, detector.Smoothing);
        }

    }

}

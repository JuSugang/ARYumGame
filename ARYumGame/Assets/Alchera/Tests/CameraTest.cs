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
    public class CameraTest
    {
        WebCamTexture webcam;

        [SetUp]
        public void InitWebcam()
        {
            WebCam.Init();
            // Do we have at least 1 camera?
            Assert.NotNull(WebCam.DeviceList);
            Assert.IsTrue(WebCam.DeviceList.Length > 0);
        }

        [TearDown]
        public void StopWebcam()
        {
            if (webcam.isPlaying)
                webcam.Stop();
        }

        [UnityTest]
        public IEnumerator CheckFront()
        {
            Assert.NotNull(WebCam.Front);
            webcam = WebCam.Front;

            Assert.NotNull(webcam);

            Assert.AreEqual(webcam.requestedWidth, WebCam.Width);
            Assert.AreEqual(webcam.requestedHeight, WebCam.Height);
            Assert.IsTrue(webcam.requestedFPS >= WebCam.FPS);

            webcam.Play();
            yield return new WaitForSeconds(2);

            Assert.IsTrue(webcam.isPlaying);
            Assert.AreEqual(webcam.width, WebCam.Width);
            Assert.AreEqual(webcam.height, WebCam.Height);
        }

#if UNITY_ANDROID || UNITY_IOS
    [UnityTest]
    public IEnumerator CheckRear()
    {
        Assert.NotNull(WebCam.Rear);
        webcam = WebCam.Rear;

        Assert.NotNull(webcam);

        Assert.AreEqual(webcam.requestedWidth, WebCam.Width);
        Assert.AreEqual(webcam.requestedHeight, WebCam.Height);
        Assert.IsTrue(webcam.requestedFPS >= 30);

        webcam.Play();
        yield return new WaitForSeconds(2);

        Assert.IsTrue(webcam.isPlaying);
        Assert.AreEqual(webcam.width, WebCam.Width);
        Assert.AreEqual(webcam.height, WebCam.Height);
    }
#endif
    }
}
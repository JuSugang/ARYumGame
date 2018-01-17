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

namespace Alchera
{
    /// <summary>
    /// Helper class for <see cref="UnityEngine.WebCamTexture"/>'s ease of use.
    /// </summary>
    public static class WebCam
    {
        public static readonly int FPS = 30;
#if UNITY_STANDALONE || UNITY_STANDALONE_OSX
        public static readonly int Width = 1280;
        public static readonly int Height = 720;
#else
        public static readonly int Width = 640;
        public static readonly int Height = 480;
#endif

        public static WebCamDevice[] DeviceList;
        public static WebCamTexture Front { get; private set; }
        public static WebCamTexture Rear { get; private set; }
        public static WebCamTexture Current { get; private set; }

        public static void Init()
        {
            // Already initialized
            if (DeviceList != null)
                return;

            DeviceList = WebCamTexture.devices;
            Debug.LogFormat("Camera Count: {0}", DeviceList.Length);
            foreach (var device in DeviceList)
            {
                if (device.isFrontFacing)
                {
                    Front = new WebCamTexture(device.name);
                    Front.requestedFPS = FPS;
                    Front.requestedWidth = Width;
                    Front.requestedHeight = Height;
                    Debug.LogFormat("Front: {0}", device.name);
                }
                else
                {
                    Rear = new WebCamTexture(device.name);
                    Rear.requestedFPS = FPS;
                    Rear.requestedWidth = Width;
                    Rear.requestedHeight = Height;
                    Debug.LogFormat("Rear: {0}", device.name);
                }
            }

            // Default camera is Rear
            if (Front == null)
            {
                Current = Rear;
                Debug.Log("Using Rear Webcam...");
            }
            else
            {
                Current = Front;
                Debug.Log("Using Front Webcam...");
            }
        }

        public static bool Swap()
        {
            // Only 1 webcam. Can do nothing
            if (DeviceList.Length < 2)
                return false;

            bool playing = Current.isPlaying;
            // Stop
            if (playing)
                Current.Stop();

            // Swap
            if (Current == Front)
                Current = Rear;
            else
                Current = Front;

            // Play again ?
            if (playing)
                Current.Play();

            return true; // Camera changed!
        }
    }

}

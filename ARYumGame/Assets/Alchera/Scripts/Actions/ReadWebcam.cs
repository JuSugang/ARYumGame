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
using UnityEngine.Profiling;

namespace Alchera
{
    public class ReadWebcam : MonoBehaviour
    {
        public Material material;
        Texture2D tex2d;
        Alchera.IFrameProcessor processor;

        void Start()
        {
            
            processor = AppData.Get<IFrameProcessor>();

            WebCam.Init();
            var webcam = WebCam.Current;

            tex2d = new Texture2D(webcam.requestedWidth, webcam.requestedHeight,
                                  TextureFormat.ARGB32, false);
            material.mainTexture = tex2d;

            webcam.Play();
        }

        void Update()
        {
            var webcam = WebCam.Current;                
            var pixels = webcam.GetPixels32();

            tex2d.SetPixels32(0,0, webcam.width, webcam.height, pixels);
            tex2d.Apply(false);

            var frame = Alchera.FrameData.Process(tex2d);
            // Get the H/W camera rotation
            uint degree = (uint)(540 - webcam.videoRotationAngle);

            Profiler.BeginSample("FrameProcess");
            if(processor != null)
                processor.OnFrame(frame, degree);
            Profiler.EndSample();
        }

        void OnDestroy()
        {
            if(WebCam.Current)
                WebCam.Current.Stop();
        }


    }

}

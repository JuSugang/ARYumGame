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
using UnityEngine.UI;
using UnityEngine.Profiling;

namespace Alchera
{
    public class ReadWebcam : MonoBehaviour
    {
        public Material material;
        Texture2D texture;

        Alchera.IFrameProcessor processor;
        Alchera.FrameData frame;

        public Text WebcamLabel;

        void Start()
        {
            frame = default(FrameData);
            processor = AppData.Get<IFrameProcessor>();

            WebCam.Init();
            var webcam = WebCam.Current;

            // Camera output texture. A material will hold it
            texture = new Texture2D(webcam.requestedWidth, webcam.requestedHeight,
                                    TextureFormat.ARGB32, false);
            material.mainTexture = texture;

            webcam.Play();  // Start the webcam
        }

        /// <summary>
        /// Read webcam pixel data.
        /// Process it with library.
        /// Forward the frame to processor.
        /// </summary>
        void Update()
        {
            var webcam = WebCam.Current;
            texture.SetPixels32(0, 0, webcam.width, webcam.height, webcam.GetPixels32());
            texture.Apply(false);

            //// Process the frame to internal format
            // frame = Alchera.FrameData.Process(tex2d: texture);
            //// If you want, you can read directly from camera with Read method
            frame.Read(width:  texture.width, 
                       height: texture.height, 
                       colors: texture.GetPixels32());

            // H/W camera rotation  
            var degree = (uint)webcam.videoRotationAngle;

            Profiler.BeginSample("FrameProcess");
            if(processor != null)
                // frame + degree + facing
                processor.OnFrame(frame, degree, webcam == WebCam.Front);
            Profiler.EndSample();
        }

        void OnDestroy()
        {
            if(WebCam.Current)
                WebCam.Current.Stop();
        }

    }

}

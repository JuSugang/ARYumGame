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

namespace Alchera
{
    public class MarkFace : MonoBehaviour, IFrameProcessor
    {
        Alchera.IFaceDetector detector;
        IFrameProcessor processor;

		public Material material;
        public Text vesionLabel;
        MeshFilter filter = null;

        void Start()
        {
            detector = Alchera.Module.Face();
            vesionLabel.text = detector.Version;

            AppData.Set<IFrameProcessor>(this);
        }

        void OnDestroy()
        {
            AppData.Set<IFrameProcessor>(null);
            detector.Dispose();
        }

        /// <summary>
        /// Frame callback. Try to detect face from it
        /// </summary>
		/// <param name="frame">Frame for detection</param>
        /// <param name="degree">Degree of current webcam</param>
        public void OnFrame(FrameData frame, uint degree)
        {
            if (detector == null)
            {
                Debug.LogError("MarkFace: Detection module is null");
                Application.Quit();
                return;
            }

            foreach (IFaceData face in detector.Faces(ref frame, (uint)degree))
            {
                OnFace(face, degree);
                break; // use only 1 face
            }
        }

        void OnFace(IFaceData face, float degree)
        {
            // Debug.Log(degree);
            var tex2d = (Texture2D)material.mainTexture;
            // Debug.Log(face.ID);

            // Draw marks
            Marker.Points(tex2d, face.Landmark, Color.green);
            tex2d.Apply(false);
        }
    }

}

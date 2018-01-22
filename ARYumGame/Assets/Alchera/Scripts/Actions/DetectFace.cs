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
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Alchera
{
    public class DetectFace : MonoBehaviour, IFrameProcessor
    {
        Alchera.IFaceDetector detector;
        MeshFilter filter;

        /// <summary>
        /// Material to show landmark
        /// </summary>
        public Material material;
        public GameObject face1;

        /// <summary>
        /// Version string on GUI
        /// </summary>
        public Text VersionLabel;

        void Start()
        {
            detector = Alchera.Module.Face();
			VersionLabel.text = detector.Version;

            // filter = face1.GetComponent<MeshFilter>();
            // filter.mesh = detector.NormalFace;
            // filter.mesh = detector.DenseFace;

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
            face1.SetActive(false);
            foreach (IFaceData face in detector.Faces(ref frame, (uint)degree))
            {
                OnFace(face, degree);
                face1.SetActive(true);
                break; // use only 1 face
            }
        }

        void OnFace(IFaceData face, float degree)
        {
            // Debug.Log(degree);
            var tex2d = (Texture2D)material.mainTexture;
            // Debug.Log(face.ID);

            //// We can save only when a face is detected
            //if (Input.GetKeyUp(KeyCode.BackQuote))
                //StartCoroutine(SaveTexture(tex2d, (int)degree));

            face.Track();

            float total = 0;
            foreach (var p in face.Animation)
            {
                total += p;
            }

            Debug.LogFormat("Shape Total: {0}, Position: {1}, Rotation: {2}", 
                            total, face.Position, 
                            face.Rotation);


            face1.transform.localPosition = face.Position;
            face1.transform.localRotation = Quaternion.AngleAxis(
                degree, Vector3.back) * face.Rotation;


            Marker.Points(tex2d, face.Landmark, Color.blue);
            tex2d.Apply(false);
        }

        IEnumerator SaveTexture(Texture2D tex2d, int degree)
        {
            yield return new WaitForEndOfFrame();
            var png = tex2d.EncodeToPNG();

            var time = System.DateTime.Now;
            var path = string.Format("{0}/{2}-{1}.png",
                                     Application.persistentDataPath,
                                     time.ToString("yyyyMMdd-HH-mmss"),
                                     degree);
            File.WriteAllBytes(path, png);
            Debug.Log(path);
        }
    }

}

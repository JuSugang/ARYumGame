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
        public Text PositionLabel;

        public GameObject glassObject;
        public GameObject headObject;

        void Start()
        {
            detector = Alchera.Module.Face();
			VersionLabel.text = detector.Version;

            filter = face1.GetComponent<MeshFilter>();
            filter.mesh = Alchera.FaceMesh.Normal;

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
            glassObject.SetActive(false);
            headObject.SetActive(false);
            foreach (IFaceData face in detector.Faces(ref frame, (uint)degree))
            {
                OnFace(face, degree);
                //face1.SetActive(true);
                glassObject.SetActive(true);
                headObject.SetActive(true);
                break; // use only 1 face
            }
        }

        void OnFace(IFaceData face, float degree)
        {
            // Debug.Log(degree);
            var tex2d = (Texture2D)material.mainTexture;
            var glassTransform = glassObject.transform;
            var headTransform = headObject.transform;
            // Draw marks
            Marker.Points(tex2d, face.Landmark, Color.green);
            tex2d.Apply(false);

            var mesh = filter.mesh;
            FaceMesh.MorphWith(mesh, face);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            var camRotation = Quaternion.AngleAxis(degree, Vector3.back);
            var rotation = camRotation * face.Rotation;
            var position = Matrix4x4.Rotate(camRotation) * face.Position;

            var webcam = WebCam.Current;
            if (webcam == WebCam.Front)
            {
                rotation.x = -rotation.x;
                rotation.y = -rotation.y;
            }
            else
            {
                position.x = -position.x;

                rotation.x = -rotation.x;
                rotation.z = -rotation.z;
            }

            position.x *= 1.1f;
            position.y *= 1.1f;
            position.z *= 0.44f;

            face1.transform.localPosition = position;
            face1.transform.localRotation = rotation;

            glassTransform.localPosition = position;
            glassTransform.localRotation = rotation;

            headTransform.localPosition = position;
            headTransform.localRotation = rotation;

            PositionLabel.text = string.Format("{0} {1}", position, rotation);
        }

    }

}

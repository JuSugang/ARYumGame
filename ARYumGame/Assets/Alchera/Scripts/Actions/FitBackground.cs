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
    public class FitBackground : MonoBehaviour
    {
#if UNITY_STANDALONE || UNITY_STANDALONE_OSX
        const float factor = 0.001627f / 2;
#else
        const float factor = 0.001627f;
#endif
        public GameObject plane;
        Quaternion rotation;
        Vector3 scale;
        Camera view;

        void Start()
        {
            // Assume this script is attached to Main Camera
            view = this.GetComponent<Camera>();

            // Initial rotation
            rotation = plane.transform.rotation;

            float distance = view.farClipPlane - 1;
            // Send the plane to FAR position 
            // But it must be closer than the cliping plane
            var position = view.transform.localPosition;
            position.z += distance;
            plane.transform.localPosition = position;

            // Scale it.
            //  the factor can be differ upon position of the plane
            scale = Vector3.one;
            scale.x = WebCam.Width * factor * distance;
            scale.y = WebCam.Height * factor * distance;

            plane.transform.localScale = scale;
        }

        void Update()
        {
            UpdateProjection();
            UpdatePlane();
        }

        void UpdateProjection()
        {
            // the value 1.920982f is from unity3d
            var Max = Mathf.Max(Screen.width, Screen.height) * 1.920982f;
            // Projection matrix
            var mat = view.projectionMatrix;
            mat[0, 0] = Max / Screen.width;
            mat[1, 1] = Max / Screen.height;
            view.projectionMatrix = mat;
        }

        void UpdatePlane()
        {
            // Apply current webcam's rotation Angle
            var webcam = WebCam.Current;

            // It's front facing. Apply mirror effect
            if(webcam == WebCam.Front){
                var mirror = new Vector3(-scale.x, scale.y, scale.z);
                plane.transform.localScale = mirror;
                plane.transform.rotation = rotation * Quaternion.AngleAxis(
                    webcam.videoRotationAngle, Vector3.forward);

            }
            else{
                plane.transform.localScale = scale;
                plane.transform.rotation = rotation * Quaternion.AngleAxis(
                    webcam.videoRotationAngle, Vector3.back);
            }
        }
    }

}

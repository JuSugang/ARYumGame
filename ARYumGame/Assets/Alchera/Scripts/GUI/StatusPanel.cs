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
    public class StatusPanel : MonoBehaviour
    {
        Text Version;
        Text Frame;
        Text Webcam;
        Text Face;
        Text Hand;

        void Start()
        {
            Text[] labels = this.GetComponentsInChildren<Text>();
            Version = labels[0];
            Frame = labels[1];
            Webcam = labels[2];
        }

        void OnGUI()
        {
            Version.text = Alchera.Module.Version;
            Frame.text = string.Format("{0} ms", Time.deltaTime * 1000);

            var webcam = WebCam.Current;
            if (webcam != null)
            {
                Webcam.text = ""
                + "WebCam:\n"
                + string.Format(" - Degree: {0} \n", webcam.videoRotationAngle)
                + string.Format(" - Size: {0} {1}\n", webcam.width, webcam.height)
                ;
            }
        }
    }

}

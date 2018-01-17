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
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Alchera
{
    public class BottomPanel : MonoBehaviour
    {
        void Start()
        {
            Button[] buttons = this.GetComponentsInChildren<Button>();
            var quit = buttons[0];
            var swap = buttons[1];

            quit.onClick.AddListener(Application.Quit);
            swap.onClick.AddListener(SwapCamera);
        }

        static void SwapCamera()
        {
            if (WebCam.Swap() == false)
                Debug.Log("Can't Swap...");
            else
                Debug.Log("Changing webcam...");
        }

    }
}
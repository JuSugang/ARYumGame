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
    public class Canvas : MonoBehaviour
    {
        public Button quit, swap;

        void Start()
        {
            Button[] buttons = this.GetComponentsInChildren<Button>();
            foreach (Button button in buttons)
            {
                button.enabled = true;
                button.onClick.RemoveAllListeners();
            }

            quit.onClick.AddListener(Application.Quit);
            swap.onClick.AddListener(SwapCamera);

            //blend.onClick.AddListener(() => { SceneManager.LoadScene("Alchera/Scenes/BlendShapeDemo"); });

            //Scene scene = SceneManager.GetActiveScene();
            //switch (scene.name)
            //{
            //    case "Alchera/Scenes/FaceDemo":
            //        face.enabled = false;
            //        break;
            //    case "Alchera/Scenes/BlendShapeDemo":
            //        blend.enabled = false;
            //        break;
            //    case "Alchera/Scenes/HandDemo":
            //        hand.enabled = false;
            //        break;
            //    default:
            //        break;
            //}
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Alchera
{
    public class SimplePanel : MonoBehaviour
    {
        Button quit, swap;

        void Start()
        {
            Button[] buttons = GetComponentsInChildren<Button>();
            foreach (Button button in buttons)
                button.onClick.RemoveAllListeners();

            quit = buttons[0];
            swap = buttons[1];

            quit.onClick.AddListener(Application.Quit);
            swap.onClick.AddListener(SimplePanel.SwapCamera);
        }

        static void SwapCamera()
        {
            if (WebCam.Swap() == false){
				Debug.Log("Can't Swap...");
                return;
            }

			Debug.Log("Changing webcam...");
        }
    }
}


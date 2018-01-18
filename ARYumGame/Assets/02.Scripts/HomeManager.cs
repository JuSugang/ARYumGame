using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Alchera;
public class HomeManager : MonoBehaviour {
    public Material capture; //quad에 붙을 메테리얼
    Texture2D tex2d;    //메테리얼에 있는 texture2d 가져올 목적으로 만든 임시 텍스쳐
    WebCamTexture webcam; //static webcam을 저장해둘 곳
    public GameObject backgroundQuad;
    void Start () {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        WebCam.Init();  //웹캠 초기화
        webcam = WebCam.Current;    //현재 웹캠을 가져옴
        webcam.Play();  //play한다.
        
        //판을 이동시킨다.
        var transform = backgroundQuad.transform;
        transform.localPosition = new Vector3(0, 0, 70);
        transform.localScale = new Vector3(-640.0f / 480, 480.0f / 480, 1) * 120;

        //텍스쳐를 만들고 메테리얼에 붙여둔다.
        tex2d = new Texture2D(webcam.requestedWidth, webcam.requestedHeight, TextureFormat.ARGB32, false);
        capture.mainTexture = tex2d;
    }

    // Update is called once per frame
    void Update () {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
        var webcam = WebCam.Current;
        var pixels = webcam.GetPixels32();
        var width = webcam.width;
        var height = webcam.height;
        tex2d.SetPixels32(0, 0, width, height, pixels);
        tex2d.Apply();
    }
    public void StartGame1()
    {
        SceneManager.LoadScene("game1Scene"); 
    }
    public void StartGame2()
    {
        SceneManager.LoadScene("game2Scene");
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            webcam.Stop();
        }
        else
        {
            webcam.Play();
        }
    }
}

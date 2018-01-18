using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour {
<<<<<<< HEAD
    public Material capture; //quad에 붙을 메테리얼
    Texture2D tex2d;    //메테리얼에 있는 texture2d 가져올 목적으로 만든 임시 텍스쳐
    WebCamTexture webcam; //static webcam을 저장해둘 곳
    public GameObject backgroundQuad;
    private bool startSureFlag=false;
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
        startSureFlag = true;
=======

	// Use this for initialization
	void Start () {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
>>>>>>> parent of 3dd783d... 홈 메뉴 블러처리
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
    }
    public void StartGame1()
    {
        SceneManager.LoadScene("game1Scene"); 
    }
    public void StartGame2()
    {
        SceneManager.LoadScene("game2Scene");
    }
<<<<<<< HEAD
    private void OnApplicationPause(bool pause)
    {
        if (startSureFlag == true)
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
=======
>>>>>>> parent of 3dd783d... 홈 메뉴 블러처리
}

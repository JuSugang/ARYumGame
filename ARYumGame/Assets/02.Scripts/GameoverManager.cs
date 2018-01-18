using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Alchera;
public class GameoverManager : MonoBehaviour {
    private int score;
    private string status;
    public Text ScoreText;
    public Material capture; //quad에 붙을 메테리얼
    Texture2D tex2d;    //메테리얼에 있는 texture2d 가져올 목적으로 만든 임시 텍스쳐
    WebCamTexture webcam; //static webcam을 저장해둘 곳
    public GameObject backgroundQuad;
    private void Start()
    {
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

        score =PlayerPrefs.GetInt("Score", 0);
        ScoreText.text = score+"";
        status = PlayerPrefs.GetString("Status", "null");
    }
    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Exit();
            }
        }
        var webcam = WebCam.Current;
        var pixels = webcam.GetPixels32();
        var width = webcam.width;
        var height = webcam.height;
        tex2d.SetPixels32(0, 0, width, height, pixels);
        tex2d.Apply();
    }
    public void Restart() //재시작 버튼을 누름
    {
        SceneManager.LoadScene(status);
    }
    public void Exit()
    {
        SceneManager.LoadScene("homeScene");
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameoverManager : MonoBehaviour {
    private int score;
    private string status;
    public Text ScoreText;
<<<<<<< HEAD
    public Material capture; //quad에 붙을 메테리얼
    Texture2D tex2d;    //메테리얼에 있는 texture2d 가져올 목적으로 만든 임시 텍스쳐
    WebCamTexture webcam; //static webcam을 저장해둘 곳
    public GameObject backgroundQuad;
    private bool startSureFlag = false;
=======
>>>>>>> parent of 3dd783d... 홈 메뉴 블러처리
    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        score =PlayerPrefs.GetInt("Score", 0);
        ScoreText.text = score+"";
        status = PlayerPrefs.GetString("Status", "null");
        startSureFlag = true;
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
    }
    public void Restart() //재시작 버튼을 누름
    {
        SceneManager.LoadScene(status);
    }
    public void Exit()
    {
        SceneManager.LoadScene("homeScene");
    }
<<<<<<< HEAD
    private void OnApplicationPause(bool pause)
    {
        if(startSureFlag==true){
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

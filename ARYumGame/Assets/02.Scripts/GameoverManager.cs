using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameoverManager : MonoBehaviour {
    private int score;
    private string status;
    public Text ScoreText;
    private void Start()
    {
        score=PlayerPrefs.GetInt("Score", 0);
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
    }
    public void Restart() //재시작 버튼을 누름
    {
        SceneManager.LoadScene(status);
    }
    public void Exit()
    {
        SceneManager.LoadScene("homeScene");
    }
}

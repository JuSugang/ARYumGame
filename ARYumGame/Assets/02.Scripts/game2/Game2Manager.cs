using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Alchera;

public class Game2Manager : MonoBehaviour
{
    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("gameoverScene");
    }
    public static Game2Manager instance; //어디서나 접근할 수 있도록 static(정적)으로 자기 자신을 저장할 변수를 만듭니다.
    public GameObject pauseview;//일시정지 했을 때 뜨는 반투명 검정 창
    public Sprite[] ReadyCount = new Sprite[5];

    public Image ReadyCountImage;   //54321 이 써지는 곳
    public GameObject ReadyImage;
    public GameObject EndImage;   //end 이미지
    public GameObject badImage; //눈에 씌울겁니다.(x x)
    public GameObject backgroundQuad; //화면을 그릴 판입니다.
    public GameObject player; //플레이어(입 shere)를 가져옵니다.
    public GameObject face1;
    public GameObject detectPanel;
    public Image detectingSign;
    public Text detectingText;
    public Sprite detectSuccess;
    public Sprite detectFail;
    public Text ScoreText; //점수를 표시하는 Text객체를 에디터에서 받아옵니다.
 

    Alchera.IFaceDetector module;
    Alchera.FrameData frame;

    public Material capture; //quad에 붙을 메테리얼
    Texture2D tex2d;    //메테리얼에 있는 texture2d 가져올 목적으로 만든 임시 텍스쳐
    WebCamTexture webcam; //static webcam을 저장해둘 곳

    private int index; //ready를 세기 위한 인덱스
    private float count = 0.001f;
    private int score = 0; //점수를 관리합니다.
    private GameObject[] bad = new GameObject[2];
    private int temp;
    MeshRenderer playerMesh;
    MeshRenderer faceMesh;
    public bool startFlag { get; set; }
    public bool pauseFlag { get; private set; }
    public bool quitSceneFlag { get; private set; }
    public bool badFlag;
    private bool detectFlag;
    public bool debugFlag;
    private bool startSureFlag = false;
    public GameObject DebugPanel;
    public Text HorTxt;
    public Text VerTxt;
    public Text TarTxt;
    public Text RealTxt;

    void Awake()
    {
        if (!instance) //정적으로 자신을 체크합니다.
            instance = this; //정적으로 자신을 저장합니다.
    }
    void Start()
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

        module = Alchera.Module.Face();
        //Debug.Log(module);
        Debug.Log(module.Version);

        startFlag = false;
        pauseFlag = false;
        debugFlag = false;
        badFlag = false;
        detectFlag = false;
        quitSceneFlag = false;
        detectPanel.SetActive(true);
        DebugPanel.SetActive(false);
        pauseview.SetActive(false); //일시정지를 안보이게 한다.
        playerMesh = player.GetComponent<MeshRenderer>();
        faceMesh = face1.GetComponent<MeshRenderer>();
        playerMesh.enabled = false;
        faceMesh.enabled = false;
        faceMesh.material.color = new Color(1, 1, 1, 0.2f);

        startSureFlag = true;
    }
    unsafe void Update()
    {
        //off
        if (module == null)//모듈이 없으면 스킵한다.
        {
            Debug.LogError("Detection module is null");
            Application.Quit();
            return;
        }

        var webcam = WebCam.Current;
        var pixels = webcam.GetPixels32();
        var width = webcam.width;
        var height = webcam.height;
        tex2d.SetPixels32(0, 0, width, height, pixels);
        tex2d.Apply();

        var degree = 540 - webcam.videoRotationAngle;
        //off
        var frame = Alchera.FrameData.Process(tex2d);
        if (pauseFlag == false)
        {
            int detectCnt = 0;
            //off
            foreach (IFaceData face in module.Faces(ref frame, (uint)degree))
            {
                this.OnFace(face, (uint)degree);
                detectCnt++;
                break;
            }
            if (detectCnt == 0)
            {
                detectingText.text = "인식실패";
                detectingSign.overrideSprite = detectFail;
                if (startFlag == false)
                {
                    detectPanel.SetActive(true);
                    ReadyImage.SetActive(false);
                }
            }
            else
            {

                detectPanel.SetActive(false);
                detectingText.text = "인식중";
                detectingSign.overrideSprite = detectSuccess;
                index = (int)Mathf.Floor(3 - count);
                
                if (index >= 0)
                {
                    ReadyImage.SetActive(true);
                    ReadyCountImage.overrideSprite = ReadyCount[index];
                    count += Time.deltaTime;
                    temp=(int)Mathf.Floor(3 - count);
                }
                if (temp < 0 && index >= 0)
                {
                    ReadyImage.SetActive(false);
                    startFlag = true;
                }

            //off
            }

        }
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Menu))
            {
                SetPause();
            }
        }
    }
    public void toggleDebug()
    {
        if (debugFlag == true)
        {
            DebugPanel.SetActive(false);
            debugFlag = false;
            playerMesh.enabled = false;
            faceMesh.enabled = false;
        }
        else
        {
            DebugPanel.SetActive(true);
            debugFlag = true;
            playerMesh.enabled = true;
            faceMesh.enabled = true;
        }
    }
    void OnDestroy()
    {
        Debug.Log("현재 스코어: " + score);
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.SetString("Status", "game2Scene");
    }
    private void OnApplicationPause(bool pause)
    {
        if (startSureFlag == true)
        {
            if (pause)
            {
                SetPause();
                webcam.Stop();
            }
            else
            {
                webcam.Play();
            }
        }
    }
    public void AddScore(int num)
    {
        if (startFlag == true)
        {
            score += num;
            ScoreText.text = score + "";
        }
    } //음식을 먹을 때 호출
    public void SetPause() {
        pauseFlag = true;
        pauseview.SetActive(true);
        Sound2Manager.instance.PauseBGM();
    } //일시정지 버튼 리스너에추가
    public void Resume() { pauseFlag = false; pauseview.SetActive(false); Sound2Manager.instance.PlayBGM(); } //재생 버튼 리스너에추가
    public void Restart() { pauseFlag = false; quitSceneFlag = true; SceneManager.LoadScene("game2Scene"); } //재시작 버튼 리스너에추가
    public void Exit() { quitSceneFlag = true; SceneManager.LoadScene("homeScene"); } //홈 버튼 리스너에추가
    public void GameOver() {
        Sound2Manager.instance.PauseBGM();
        quitSceneFlag = true;
        EndImage.SetActive(true);
        StartCoroutine(NextScene());
    } //시간이 다된경우 호출

    public void OnFace(IFaceData face, uint degree)
    {

        face.Track();
        // Draw marks
        Vector2[] points = face.Landmark;
        if (debugFlag == true)
        {
            DrawPoint(tex2d, points[104], Color.red);
            DrawPoint(tex2d, points[105], Color.blue);
            for (int i = 96; i < 104; i++)
            {
                DrawPoint(tex2d, points[i], Color.green);
            }

            DrawPoint(tex2d, points[2], new Color(1, 0, 1, 1));
            DrawPoint(tex2d, points[30], new Color(1, 0, 1, 1));
            DrawPoint(tex2d, points[16], new Color(1, 0, 1, 1));
        }
            //입 좌우상하
        float x1 = points[96].x;    
        float y1 = points[96].y;
        float x2 = points[100].x;
        float y2 = points[100].y;
        float x3 = points[98].x;
        float y3 = points[98].y;
        float x4 = points[102].x;
        float y4 = points[102].y;
        float HorRadius = Mathf.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        float VerRadius = Mathf.Sqrt((x3 - x4) * (x3 - x4) + (y3 - y4) * (y3 - y4));
        float centerXInView = (x1 + x2) / 2 - 320;
        float centerYInView = (y1 + y2) / 2 - 240;
        float resizeX = centerXInView * (float)24 / 250;
        float resizeY = centerYInView * (float)15.8 / 150;


        //-----------------입 그리기--------------
        Vector3 pos = new Vector3(-1 * resizeX, resizeY, 20);
        player.transform.position = pos;
        player.transform.localScale = new Vector3(HorRadius / 12, VerRadius / 8, 1);
        Quaternion PlayerAngle = Quaternion.Euler(0, 0, Mathf.Atan2(y2 - y1, x1 - x2) * 180 / Mathf.PI);
        player.transform.rotation = PlayerAngle;


        //입 열고닫음 여부 확인
        if (VerRadius / HorRadius > 0.4 && startFlag == true)
        {
            Player2.instance.mouseOpen = true;
        }
        else
        {
            Player2.instance.mouseOpen = false;
        }
        //----------------눈에 x 그리기--------------
        float eyeCenterX=0;
        float eyeCenterY=0;
        for (int i = 0; i < 2; i++)
        {
            float eyeX = (points[104 + i].x - 320) * (float)24 / 250;
            float eyeY = (points[104 + i].y - 240) * (float)15.8 / 150;
            Vector3 eyepos = new Vector3(-1 * eyeX, eyeY, 22);
            if (badFlag == true)
            {
                Quaternion angle = Quaternion.Euler(0, 0, 0);
                bad[i] = (GameObject)Instantiate(badImage, eyepos, angle);
            }
            if (bad[i] != null)
            {
                bad[i].transform.position = eyepos;
                bad[i].transform.localScale = new Vector3(HorRadius / 17, HorRadius / 17, 1);
            }
            eyeCenterX += eyeX / 2;
            eyeCenterX += eyeY / 2;
        }
        badFlag = false;

        //-----------------얼굴그리기-----------
        float x5 = points[2].x;
        float y5 = points[2].y;
        float x6 = points[30].x;
        float y6 = points[30].y;
        float cenX = (x5 + x6) / 2;
        float cenY = (y5 + y6) / 2;
        float FaceHorRadius = Mathf.Sqrt((x5 - x6) * (x5 - x6) + (y5 - y6) * (y5 - y6))/2;
        float FaceVerRadius = Mathf.Sqrt((cenX - points[16].x) * (cenX - points[16].x) + (cenY - points[16].y) * (cenY - points[16].y));
        float centerXInFView = cenX - 320;
        float centerYInFView = cenY - 240;
        float resizeFX = centerXInFView * 0.12f;
        float resizeFY = centerYInFView * 0.12f;

        face1.transform.localPosition = new Vector3(-resizeFX, resizeFY,30);
        face1.transform.localScale = new Vector3(FaceHorRadius / 3.8f, FaceVerRadius / 3.8f, 1);
        Quaternion faceAngle = Quaternion.Euler(0, 0, Mathf.Atan2(y6 - y5, x5 - x6) * 180 / Mathf.PI);
        face1.transform.rotation = faceAngle;


        //-----------------디버그 창------------
        HorTxt.text = "x좌표 : " + string.Format("{0:F1} {1:F1}", x5, x6);
        VerTxt.text = "y좌표 : " + string.Format("{0:F1} {1:F1}", y5, y6);
        TarTxt.text = "x평균 : " + string.Format("{0:F1}", (y5+y6)/2);
        RealTxt.text = "실제비율 : " + string.Format("{0:F2}", FaceVerRadius / FaceHorRadius); ;
        tex2d.Apply();
    }

    public void DrawPoint(Texture2D tex2d, Vector2 point, Color color)
    {
        int x = (int)point.x;
        int y = (int)point.y;

        tex2d.SetPixel(x + 2, y + 1, color);
        tex2d.SetPixel(x + 1, y + 1, color);
        tex2d.SetPixel(x + 0, y + 1, color);
        tex2d.SetPixel(x - 1, y + 1, color);
        tex2d.SetPixel(x - 2, y + 1, color);

        tex2d.SetPixel(x + 2, y + 0, color);
        tex2d.SetPixel(x + 1, y + 0, color);
        tex2d.SetPixel(x + 0, y + 0, color);
        tex2d.SetPixel(x - 1, y + 0, color);
        tex2d.SetPixel(x - 2, y + 0, color);

        tex2d.SetPixel(x + 2, y - 1, color);
        tex2d.SetPixel(x + 1, y - 1, color);
        tex2d.SetPixel(x + 0, y - 1, color);
        tex2d.SetPixel(x - 1, y - 1, color);
        tex2d.SetPixel(x - 2, y - 1, color);
    }
}

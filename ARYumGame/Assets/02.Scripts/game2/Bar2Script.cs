using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar2Script : MonoBehaviour
{
    public static Bar2Script instance;
    private float currentTime;
    public float playingTime;
    public GameObject clock;
    public GameObject timebar;
    public GameObject barEnd;
    private float posX;
    private float posY;
    private float paneltyTriggerCount;
    void Awake()
    {
        if (!instance) //정적으로 자신을 체크합니다.
            instance = this; //정적으로 자신을 저장합니다.
    }
    private void Start()
    {
        currentTime = 0;
        posX = timebar.transform.position.x;
        posY = timebar.transform.position.y;
        paneltyTriggerCount = 1;
    }
    // Update is called once per frame
    void Update()
    {
        //ready상태가 아니고, 일시정지 상태가 아닐때 (순수 play 시간일때)
        if (Game2Manager.instance.startFlag == true && Game2Manager.instance.pauseFlag != true)
        {
            currentTime += Time.deltaTime;
            if (currentTime > playingTime)
            {
                Game2Manager.instance.startFlag=false;
                Game2Manager.instance.GameOver();
            }
            else
            {
                timebar.transform.position=new Vector3(posX-currentTime*1.3f*posX/playingTime, posY, 0);
            }
            if (paneltyTriggerCount < 1 )
            {
                paneltyTriggerCount += Time.deltaTime*3;
            }
            else
            {
                paneltyTriggerCount = 1;
            }
            barEnd.GetComponent<Image>().color = new Color(paneltyTriggerCount/2 + 0.5f, paneltyTriggerCount, paneltyTriggerCount, 1);
            clock.GetComponent<Image>().color = new Color(paneltyTriggerCount / 2 + 0.5f, paneltyTriggerCount, paneltyTriggerCount, 1);
            gameObject.GetComponent<Image>().color = new Color(paneltyTriggerCount / 2+0.5f, paneltyTriggerCount, paneltyTriggerCount, 1);
        }
    }
    public void TimePanelty(float num)
    {
        paneltyTriggerCount = 0;
        currentTime += num;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food2Manager : MonoBehaviour
{
    // Use this for initialization
    private float lifeCycle; //음식이 존재하는 시간 
    private float count; //시간 세는데 씀
    private bool paleFlag = false;
    private MeshRenderer MeshComponent; //투명하다가 count>=5 후에 투명도를 100%으로 할 것이다.

    public int myScore;
    public GameObject Score; //먹히고 나서 생성할 점수들 (1~9,-1~-9)
    public GameObject explosion;

    void Start()
    {
        count = 0;
        lifeCycle = 400;
    }

    // Update is called once per frame
    void Update()
    {
        if (Game2Manager.instance.pauseFlag == false)
        {
            count += 1;
        }
        
        if (lifeCycle < count)
        {   //아무것도 하지 않고 일정 시간이 지나면 상했다고 가정하여 자기자신을 삭제한다.
            paleFlag = true;
            Destroy(gameObject);
        }

    }
    private void OnDestroy()
    {
        if (paleFlag == false)
        { //상하지 않았는데 파괴되면, 먹힌것으로 가정하여 현재 위치에 점수를 띄우고, 점수를 합산한다.
            Vector3 pos = transform.position;
            Quaternion angle = Quaternion.Euler(0, 0, 0);
            if (myScore > 0)
            {
                GameObject explode = (GameObject)Instantiate(explosion, pos + new Vector3(0, 3, 1), angle);
            }
            else
            {
                GameObject explode = (GameObject)Instantiate(explosion, pos + new Vector3(0, 3, 1), angle);
                Game2Manager.instance.badFlag = true;
                Bar2Script.instance.TimePanelty(1);
            }
            GameObject score = (GameObject)Instantiate(Score, pos+ new Vector3(0, 2, 0), angle);
            Game2Manager.instance.AddScore(myScore);
        }
    }
}

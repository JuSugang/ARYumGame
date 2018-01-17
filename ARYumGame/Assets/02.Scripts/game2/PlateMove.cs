using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateMove : MonoBehaviour {
    public float Speed;
    public float StartTime;
    private int lifeCycle;
    private int count;
    private int paleCount;
    private bool moveFlag;
    void Start()
    {
        count = 0;
        paleCount = 0;
        Speed = 13f;
        StartTime = 50f;
        moveFlag = true;
        lifeCycle = 500;
        
    }
	void Update () {

        if (Game2Manager.instance.startFlag == true && Game2Manager.instance.pauseFlag != true)
        {
            
            paleCount++;
            if (count < StartTime)
            {
                count++;
            }
            if (lifeCycle < paleCount)
            {   //아무것도 하지 않고 일정 시간이 지나면 상했다고 가정하여 자기자신을 삭제한다.
                Destroy(gameObject);
            }
            else
            {
                if (moveFlag == true)
                {
                    transform.Translate(new Vector3(0, 0, 1) * Speed*Time.deltaTime);
                }
            }
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "face"&&this.gameObject.layer==14)
        {
            this.gameObject.layer = 8;
            this.transform.GetChild(0).gameObject.layer = 8;
            moveFlag = false;
        }
    }

}

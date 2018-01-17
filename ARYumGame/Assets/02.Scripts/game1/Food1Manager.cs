using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food1Manager : MonoBehaviour {
    // Use this for initialization
    private float lifeCycle; //음식이 존재하는 시간 
    private float count; //시간 세는데 씀
    private bool aliveFlag = false; 
    private bool paleFlag = false;
    private MeshRenderer MeshComponent; //투명하다가 count>=5 후에 투명도를 100%으로 할 것이다.
    public int myScore;
    public GameObject Score; //먹히고 나서 생성할 점수들 (1~9,-1~-9)
    public GameObject explosion;
    public GameObject Hightlight;
    void Start () {
        MeshComponent = gameObject.GetComponent<MeshRenderer>();
        MeshComponent.enabled = false; //생성은 했지만, 아직 렌더링은 하지 않는다. 
        count = 0;
        lifeCycle = 200;
        Hightlight.GetComponent<MeshRenderer>().enabled=false;
    }
	
	// Update is called once per frame
	void Update () {
        if (Game1Manager.instance.pauseFlag ==false && Game1Manager.instance.startFlag == true)
        {
            transform.Rotate(Time.deltaTime * 50, 0, 0);
            count += 1;
        }
        if (count > 5)
        {   //5count까지 버텼다면, 주변에 충돌이 일어나는 물체가 없다고 판단하여 랜더링을 한다.
            aliveFlag = true;
            MeshComponent.enabled = true;
            
        }
        if (lifeCycle < count) 
        {   //아무것도 하지 않고 일정 시간이 지나면 상했다고 가정하여 자기자신을 삭제한다.
            paleFlag = true;
            Destroy(gameObject);
        }
        
        if (this.tag == "eatting")
        {
            Debug.Log(this.tag);
            Hightlight.GetComponent<MeshRenderer>().enabled = true;
        }
    }
    void OnTriggerEnter(Collider otherObj)
    {   //생성 시 부딛히면, 생성되는 obj를 바로 파괴한다.

        if (otherObj != Hightlight&&aliveFlag == false)
        {
            Destroy(gameObject);

        }
        
    }
    private void OnDestroy()
    {
        if(aliveFlag == true && paleFlag==false)
        { //상하지 않았는데 파괴되면, 먹힌것으로 가정하여 현재 위치에 점수를 띄우고, 점수를 합산한다.
            Vector3 pos= transform.position;
            Quaternion angle = Quaternion.Euler(0,0,0);
            if (myScore > 0)
            {
                GameObject explode = (GameObject)Instantiate(explosion, pos + new Vector3(0, 2, 1), angle);
            }
            else
            {
                GameObject explode = (GameObject)Instantiate(explosion, pos + new Vector3(0, 0, 1), angle);
                Game1Manager.instance.badFlag = true;
                Bar1Script.instance.TimePanelty(1);
            }
            GameObject score = (GameObject)Instantiate(Score, pos, angle);
            Game1Manager.instance.AddScore(myScore);
        }
    }
}

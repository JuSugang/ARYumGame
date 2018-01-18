using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn1Manager : MonoBehaviour {
    
    public GameObject[] goodFood =new GameObject[15]; //Prefab을 받을 public 변수 입니다.
    public GameObject[] badFood = new GameObject[10]; //Prefab을 받을 public 변수 입니다.
    public float SpawnSpeed; //음식이 나오는 속도

    private GameObject food;
    private int index; 
    private float SpawnGoodTime; //좋은 음식이 나오는 시간간격
    private float SpawnBadTime; //나쁜 음식이 나오는 시간간격

    private void Start()
    {
        SpawnGoodTime = Random.Range(30f, 50f);
        SpawnBadTime = Random.Range(100f, 200f);
    }
    void Update()
    {
        //ready상태가 아니고, 일시정지 상태가 아닐때 (순수 play 시간일때)
        if (Game1Manager.instance.startFlag == true&& Game1Manager.instance.pauseFlag == false)
        {
            SpawnGoodTime -= SpawnSpeed;
            SpawnBadTime -= SpawnSpeed;
            if (SpawnGoodTime < 0)
            {
                SpawnFood(true); //좋은 음식을 생성한다.
                SpawnGoodTime = Random.Range(30f, 50f);
            }
            if (SpawnBadTime < 0)
            {
                SpawnFood(false); //나쁜 음식을 생성한다.
                SpawnBadTime = Random.Range(100f, 200f);
            }
        }
    }

    void SpawnFood(bool flag)
    {
        //위치,회전각,물체 랜덤설정
        float randomX = Random.Range(-20f, 20f); 
        float randomY = Random.Range(-11f, 3f);
        Quaternion angle = Quaternion.Euler(Random.Range(-90f, 90f), Random.Range(-90f, 90f), Random.Range(-20f, 20f));
        //생성된 값으로 food를 만들고, 적당히 키워준다.

        if (flag == true)
        {
            index = (int)Mathf.Floor(Random.Range(0f, 14.9f));
            food = (GameObject)Instantiate(goodFood[index], new Vector3(randomX, randomY, 20f), angle);
        }
        else
        {
            index = (int)Mathf.Floor(Random.Range(0f, 9.9f));
            food = (GameObject)Instantiate(badFood[index], new Vector3(randomX, randomY, 20f), angle);
        }

        food.transform.localScale = new Vector3(40f, 40f, 40f);
    }
    
    
}

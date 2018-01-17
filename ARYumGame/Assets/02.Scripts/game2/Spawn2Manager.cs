using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Spawn2Manager : MonoBehaviour {
    public GameObject[] goodFood = new GameObject[15]; //Prefab을 받을 public 변수 입니다.
    public GameObject[] badFood = new GameObject[10]; //Prefab을 받을 public 변수 입니다.
    
    public TextAsset nodeData;
    private GameObject food;
    private ArrayList nodes = new ArrayList();
    private int index;
    private int curPos;
    private float timer;
    public float spawnCycle;
    // Use this for initialization
    void Start () {
        ReadNodeData();
        timer = spawnCycle;
        curPos = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (Game2Manager.instance.startFlag == true && Game2Manager.instance.pauseFlag != true)
        {
            timer += Time.deltaTime;
            if (timer > spawnCycle)
            {
                if (curPos < nodes.Count)
                {
                    SpawnFood(curPos);
                    curPos++;
                }
                timer = 0;
            }
        }
    }
    void SpawnFood(int curPos)
    {
        string source = (string)nodes[curPos];
        string[] node = source.Split(',');
        for (int i = -1; i < 2; i++)
        {
            float posX = i * 9f;
            float posY = -6.8f;
            float posZ = -8f;
            Quaternion angle = Quaternion.Euler(0, 0, 0);
            if (node[i + 1] == "a")
            {
                index = (int)Random.Range(0f, 14.9f);
                food = (GameObject)Instantiate(goodFood[index], new Vector3(posX, posY, posZ), angle);
            }
            if (node[i + 1] == "b")
            {
                index = (int)Random.Range(0f, 9.9f);
                food = (GameObject)Instantiate(badFood[index], new Vector3(posX, posY, posZ), angle);
            }
            if (food != null)
            {
                food.transform.localScale = new Vector3(30f, 30f, 30f);
            }
        }

    }
    public void ReadNodeData()
    {
        StringReader sr = new StringReader(nodeData.text);
        string source = sr.ReadLine();
        while (source != null)
        {
            nodes.Add(source);
            if (source.Length == 0)
            {
                sr.Close();
                return;
            }
            source = sr.ReadLine();
        }
    }
}

using UnityEngine;
using System.Collections;

public class Lane : MonoBehaviour
{
    public GameObject[] outline = new GameObject[3];

    // Use this for initialization
    void Start()
    {
        for(int i = 0; i < 3; i++)
        {
            outline[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.tag == "Lane")
        {
            for (int i = 0; i < 3; i++)
            {
                outline[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                outline[i].SetActive(true);
                if (Player2.instance.mouseOpen == true)
                {
                    outline[i].GetComponent<MeshRenderer>().material.SetColor("_TintColor",new Color(0, 1, 0.5f, 0.3f));
                }
                else
                {
                    outline[i].GetComponent<MeshRenderer>().material.SetColor("_TintColor", new Color(1, 1 , 1, 0.1f));
                }
            }
            
        }
    }
}

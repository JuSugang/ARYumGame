using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    public static Player2 instance;
    MeshRenderer MeshComponent;
    public bool mouseOpen;
    void Awake()
    {
        if (!instance) //정적으로 자신을 체크합니다.
            instance = this; //정적으로 자신을 저장합니다.
    }
    // Use this for initialization
    void Start()
    {
        MeshComponent = gameObject.GetComponent<MeshRenderer>();
        MeshComponent.enabled = false;
        mouseOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        move();
        if (mouseOpen)
        {
            MeshComponent.material.color = new Color(0, 1, 0, 0.4f);
        }
        else
        {
            MeshComponent.material.color = new Color(1, 1, 1, 0.4f);
        }
    }
    void OnTriggerStay(Collider otherObj)
    {
        if (otherObj.tag == "Lane")
        {
            otherObj.tag = "LaneHover";
        }
        if(otherObj.tag != "Lane"&& otherObj.tag != "LaneHover"&&otherObj.tag!="LayerChanger")
        {   
            if (mouseOpen) {
                Destroy(otherObj.gameObject);
            }
        }
    }
    void OnTriggerExit(Collider otherObj)
    {
        if(otherObj.tag == "LaneHover")
        {
            otherObj.tag = "Lane";
        }
    }
    void move()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseOpen = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            mouseOpen = false;
        }
        Vector3 mousePos = Input.mousePosition;
        float speedX = 60;
        float speedY = 35;
        transform.position = new Vector3((mousePos.x / Screen.width - 0.5f) * speedX, (mousePos.y / Screen.height - 0.5f) * speedY, 20f);
    }
}

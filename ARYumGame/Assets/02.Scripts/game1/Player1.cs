using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour {
    public static Player1 instance;
    MeshRenderer MeshComponent;
    public bool mouseOpen;
    void Awake()
    {
        if (!instance) //정적으로 자신을 체크합니다.
            instance = this; //정적으로 자신을 저장합니다.
    }
    // Use this for initialization
    void Start () {
        MeshComponent = gameObject.GetComponent<MeshRenderer>();
        MeshComponent.enabled = false;
       
        mouseOpen = false;
    }
	
	// Update is called once per frame
	void Update () {
        move();
        if (mouseOpen)
        {
            MeshComponent.material.color = new Color(0, 1, 0, 0.2f);
        }
        else
        {
            calEaten();
            MeshComponent.material.color = new Color(1, 1, 1, 0.2f);
        }
	}
    public void togglemesh()
    {
        
        if(MeshComponent.enabled == false)
        {
            MeshComponent.enabled = true;
        }
        else
        {
            MeshComponent.enabled = false;
        }
    }
    void OnTriggerStay(Collider otherObj)
    {
        if (mouseOpen)
        {
            otherObj.tag = "eatting";
        }
    }
    void calEaten()
    {
        GameObject[] foods = GameObject.FindGameObjectsWithTag("eatting");
        foreach (GameObject food in foods)
            GameObject.Destroy(food);

    }
    void move()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MeshComponent.material.color = new Color(0, 1, 0, 0.2f);
            mouseOpen = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            mouseOpen = false;
            calEaten();
            MeshComponent.material.color = new Color(1, 1, 1, 0.2f);
        }
        Vector3 mousePos = Input.mousePosition;
        float speedX = 60;
        float speedY = 35;
        transform.position = new Vector3((mousePos.x / Screen.width - 0.5f)*speedX, (mousePos.y / Screen.height - 0.5f)*speedY, 20f);
    }
}

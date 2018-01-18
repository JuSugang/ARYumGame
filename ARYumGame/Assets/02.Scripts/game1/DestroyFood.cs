using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFood : MonoBehaviour
{

    private int count;
    private int lifeCycle;
    void Start()
    {
        count = 0;
        lifeCycle = 400;
    }
    private void Update()
    {

        if (lifeCycle < count)
        {
            Destroy(gameObject);
        }
        else
        {
            count++;
        }
    }
}

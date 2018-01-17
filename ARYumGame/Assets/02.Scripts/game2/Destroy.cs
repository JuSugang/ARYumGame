using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{

    private float DestroyTime = 1.0f;
    void Start()
    {
        Destroy(gameObject, DestroyTime);
    }
}

using UnityEngine;
using System.Collections;

public class LayerChange : MonoBehaviour
{

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            other.gameObject.layer = 14;
        }
    }
}

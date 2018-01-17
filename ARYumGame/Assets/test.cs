using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class test : MonoBehaviour {
    Text m_Text;
    // Use this for initialization
    void Start () {
        m_Text = GetComponent<Text>();
        m_Text.fontSize = 50;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

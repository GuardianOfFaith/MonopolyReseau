//Florent WASSEN
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageBoxScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CloseMsgBox()
    {
        gameObject.SetActive(false);
    }
}

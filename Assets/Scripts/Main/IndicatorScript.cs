using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorScript : MonoBehaviour {

    private float distance = 0.0f;
    private bool direction = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (direction && distance < 1)
        {
            distance += 0.05f;
            Vector3 pos = gameObject.transform.position;
            pos.y -= 0.05f;
            gameObject.transform.position = pos;
        }
        else if (!direction && distance > 0)
        {
            distance -= 0.05f;
            Vector3 pos = gameObject.transform.position;
            pos.y += 0.05f;
            gameObject.transform.position = pos;
        }
        else
        {
            direction ^= true;
        }
    }
}

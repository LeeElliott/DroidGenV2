using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Use this for initialization
	void Start()
    {
		
	}
	
	// Update is called once per frame
	void Update()
    {
        PlayerControl();
	}

    // Player control methods here
    void PlayerControl()
    {
        // Move Forward
        if (Input.GetKey("w"))
        {
            transform.Translate(0.0f, 0.0f, 1.0f);
        }
        // Move Back
        if (Input.GetKey("s"))
        {
            transform.Translate(0.0f, 0.0f, -1.0f);
        }
        // Turn Right
        if (Input.GetKey("d"))
        {
            transform.Rotate(0.0f, 1.0f, 0.0f);
        }
        // Turn Left
        if (Input.GetKey("a"))
        {
            transform.Rotate(0.0f, -1.0f, 0.0f);
        }
    }
}

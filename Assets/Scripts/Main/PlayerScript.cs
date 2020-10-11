using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private float moveSpeed = 4.0f;
    private float turnSpeed = 2.5f;
    private bool move = true;

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

        //Move Front/Back
        if (MobileJoystick_UI.instance.moveDirection.y != 0)
        {
            if (move)
                transform.Translate(transform.forward * Time.deltaTime * moveSpeed * MobileJoystick_UI.instance.moveDirection.y, Space.World);
        }

        //Rotate Left/Right
        if (MobileJoystick_UI.instance.moveDirection.x != 0)
        {
            if (move)
                transform.Rotate(new Vector3(0, 14, 0) * Time.deltaTime * turnSpeed * MobileJoystick_UI.instance.moveDirection.x, Space.Self);
        }
    }

    //Detect collisions between the GameObjects with Colliders attached
    void OnCollisionEnter(Collision collision)
    {
        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (collision.gameObject.name == "Water")
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            move = false;
        }
    }
}

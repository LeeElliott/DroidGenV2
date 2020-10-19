using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEffectScript : MonoBehaviour
{
    public Transform droidText;
    public Transform genText;
    public Transform anText;
    public Transform eratorText;
    public Transform startButton;
    public Transform creditsButton;
    public Transform infoButton;
    public Transform logo;

    private bool falling = false;
    private float dropping = 0.0f;

	// Use this for initialization
	void Start ()
    {
        startButton.gameObject.SetActive(false);
        creditsButton.gameObject.SetActive(false);
        infoButton.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (genText.transform.position.x > 2)
        {
            Vector3 droidPos = genText.transform.position;
            droidPos.x -= 0.5f;
            genText.transform.position = droidPos;
        
            Vector3 genPos = droidText.transform.position;
            genPos.x += 0.5f;
            droidText.transform.position = genPos;
        }
        else
        {
            startButton.gameObject.SetActive(true);
            creditsButton.gameObject.SetActive(true);
            infoButton.gameObject.SetActive(true);
            falling = true;
        }

        if (falling)
        {
            Vector3 anRot = anText.transform.rotation.eulerAngles;
            Vector3 eratorRot = eratorText.transform.rotation.eulerAngles;
                
            if (anText.transform.rotation.z < 90)
            {
                anRot.z += 3f;
                anText.transform.rotation = Quaternion.Euler(anRot);
                anText.transform.position = new Vector3(anText.transform.position.x, 
                    anText.transform.position.y - dropping, anText.transform.position.z);

                eratorRot.z -= 3f;
                eratorText.transform.rotation = Quaternion.Euler(eratorRot);
                eratorText.transform.position = new Vector3(eratorText.transform.position.x,
                    eratorText.transform.position.y - dropping, eratorText.transform.position.z);

                dropping += 0.02f;
            }
        }

        Vector3 logoRot = logo.transform.rotation.eulerAngles;
        logoRot.y += 0.5f;

        logo.transform.rotation = Quaternion.Euler(logoRot);
    }
}

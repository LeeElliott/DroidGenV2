using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEffectScript : MonoBehaviour
{
    public Transform title;
    public Transform startButton;
    public Transform creditsButton;
    public Transform infoButton;

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
        if (title.transform.localScale.x < 3)
        {
            title.transform.localScale = new Vector3(title.transform.localScale.x + 0.018f, title.transform.localScale.y + 0.012f, 1); 

        }
        else
        {
            startButton.gameObject.SetActive(true);
            creditsButton.gameObject.SetActive(true);
            infoButton.gameObject.SetActive(true);
        }
    }
}

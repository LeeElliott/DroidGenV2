using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    // Click event for start button
	public void StartClicked()
    {
        SceneManager.LoadScene("World");
    }

    // Click event for the credits button
    public void CreditsClicked()
    {
        SceneManager.LoadScene("Credits");
    }

    public void InfoClicked()
    {
        SceneManager.LoadScene("Info");
    }

    // Click event for the back button on credits screen
    public void MenuClicked()
    {
        SceneManager.LoadScene("Menu");
    }
}

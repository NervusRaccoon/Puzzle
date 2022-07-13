using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResolutionButtons: MonoBehaviour
{
    public void ClickOnPlayButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void PressOnExitButton()
    {
        Application.Quit(); 
    }
}

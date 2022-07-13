using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinalButton : MonoBehaviour
{
    private Color normal = new Color(1f,1f,1f,1f);
    public GameObject button;
    private Color lastCol = new Color(1f,1f,1f,0f);
    private void LateUpdate() 
    {
        if (lastCol != normal)
        {
            lastCol = button.GetComponent<Image>().color;
            button.GetComponent<Image>().color = Color.Lerp(lastCol, normal, Time.deltaTime*0.5f);
        }
    }

    public void ClickOnPlayButton()
    {
        SceneManager.LoadScene("LevelMenu", LoadSceneMode.Single);
    }
}

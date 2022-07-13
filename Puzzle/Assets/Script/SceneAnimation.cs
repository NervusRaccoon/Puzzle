using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneAnimation : MonoBehaviour
{
    private Color lastCol;
    private Color empty = new Color(1f,1f,1f,0f);
    private Color normal = new Color(0f,0f,0f,1f);
    private bool noDark = false;

    void LateUpdate()
    {
        if (!noDark)
        {
            if (lastCol != empty)
            {
                lastCol = gameObject.GetComponent<Image>().color;
                gameObject.GetComponent<Image>().color = Color.Lerp(lastCol, empty, Time.deltaTime*4f);
            }
            else
            {
                noDark = true;
            }
        }
        else
        {
            lastCol = gameObject.GetComponent<Image>().color;
            if (lastCol == normal)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            gameObject.GetComponent<Image>().color = Color.Lerp(lastCol, normal, Time.deltaTime*8f);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public static GameObject nextLevelButton;
    public static Sprite unactiveLevel;
    public static Sprite activeLevel;
    public static GameObject img = null;
    private const int sublevelNumber = 5;
    void Start()
    {
        nextLevelButton = GameObject.Find("NextLevelButton");  
        nextLevelButton.SetActive(false);

        activeLevel = Resources.Load<Sprite>("Sprite/Interface/active_sublevel");
        unactiveLevel = Resources.Load<Sprite>("Sprite/Interface/unactive_sublevel");

        GameObject.Find("1").GetComponent<Image>().sprite = activeLevel;
    }

    public void PressedAutoCompleteButton()
    {
        if (!Swap.win)
        {
            Swap.win = true;
            nextLevelButton.SetActive(true);
            LevelController.AutoComplete();
        }
    }

    public void PressedNextLevelButton()
    {
        LoadNewLevel(LevelController.currentLevelIndex+1);
        nextLevelButton.SetActive(false);
    }

    private void LoadNewLevel(int index)
    {
        CleanPrevLevel();
        var lastIndex = LevelController.LoadLevelData(index);
        GameObject.Find((lastIndex % sublevelNumber + 1).ToString()).GetComponent<Image>().sprite = unactiveLevel;
        GameObject.Find((LevelController.currentLevelIndex % sublevelNumber + 1).ToString()).GetComponent<Image>().sprite = activeLevel;
    }

    public static void CleanPrevLevel()
    {
        if (SceneManager.GetActiveScene().name == "Gameplay")
        {
            LevelController.CleanPieces();
            LevelController.CleanLevelData();
            foreach(Transform child in GameObject.Find("Sublevels").transform)
            {
                child.gameObject.GetComponent<Image>().sprite = unactiveLevel;
            }
            DestroyImmediate(GameObject.Find("Grid"));
            Swap.win = false;
        }
    }

    public void PressedSublevelButton()
    {
        string nameButton = EventSystem.current.currentSelectedGameObject.name;
        int i = int.Parse(nameButton);
        i = LevelController.currentLevelIndex - (LevelController.currentLevelIndex % sublevelNumber) + i - 1;

        LoadNewLevel(i);
        nextLevelButton.SetActive(false);
    }
    public static void FinalLevel()
    {
        CleanPrevLevel();
        SceneManager.LoadScene("FinalLevel", LoadSceneMode.Single);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PressedAutoCompleteButton();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            PressedNextLevelButton();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            LoadNewLevel(LevelController.currentLevelIndex-1);
            nextLevelButton.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            int index = (LevelController.currentLevelIndex-LevelController.currentLevelIndex%5);
            LoadNewLevel(index - 5);
            nextLevelButton.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            int index = (LevelController.currentLevelIndex-LevelController.currentLevelIndex%5);
            LoadNewLevel(index + 5);
            nextLevelButton.SetActive(false);
        }
        if (Swap.win)
        {
            nextLevelButton.SetActive(true);
            LevelController.AutoComplete();
            //Swap.win = false;
        }
    }
}

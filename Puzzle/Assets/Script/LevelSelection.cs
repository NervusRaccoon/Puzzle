using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LevelSelection : MonoBehaviour
{
    private string path;
    private Color closed = new Color(0.2f, 0.2f, 0.2f, 1f);
    public static GameObject[] levelList = new GameObject[levelCount];
    private const int levelCount = 8;
    void Start()
    {
        //levelList = GameObject.FindGameObjectsWithTag("LevelMenu");
        if (levelList[0] == null)
        {
            for (int i = 0; i < levelCount; i++)
            {
                levelList[i] = GameObject.Find((i+1).ToString());
            }
        }

        path = Application.streamingAssetsPath + "/data.json";

        var jsonString = File.ReadAllText(path);
        LevelController.levelStruct[] levelData = LevelController.JsonHelper.FromJson<LevelController.levelStruct>(jsonString);

        levelList[0].transform.GetChild(0).gameObject.SetActive(false);

        for (int i = 0; i < levelData.Length; i++)
        {
            if (levelData[i].sublevel == 5 
                && levelData[i].level != levelData[levelData.Length-1].level 
                && levelData[i].passed == true)
            {
                levelList[levelData[i+1].level-1].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < levelList.Length; i++)
        {
            levelList[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(levelData[i*5].imagePath);
            if (levelList[i].transform.GetChild(0).gameObject.activeSelf)
            {
                levelList[i].GetComponent<Button>().interactable = false;
                levelList[i].GetComponent<Image>().color = closed;
            }
        }
    }

    public void PressLevelButton()
    {
        string nameButton = EventSystem.current.currentSelectedGameObject.name;
        int i = int.Parse(nameButton);
        i = (i-1)*5;

        LevelController.currentLevelIndex = i;
        SceneManager.LoadScene("Gameplay", LoadSceneMode.Single);
    }
}

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GalleryController : MonoBehaviour
{
    private List<GameObject> levelList = new List<GameObject>();
    private List<GameObject> sublevelList = new List<GameObject>(); 
    private Sprite unactiveLevelspr;
    private Sprite activeLevelspr; 
    private Sprite unactiveSublevelspr;
    private Sprite activeSublevelspr; 
    private int activeLevel = 0;
    private int activeSublevel = 0;
    private LevelController.levelStruct[] levelData;  
    void Start()
    {
        activeSublevelspr = Resources.Load<Sprite>("Sprite/Interface/active_sublevel");
        unactiveSublevelspr = Resources.Load<Sprite>("Sprite/Interface/unactive_sublevel");
        activeLevelspr = Resources.Load<Sprite>("Sprite/Interface/active_level");
        unactiveLevelspr = Resources.Load<Sprite>("Sprite/Interface/unactive_level");

        string path = Application.streamingAssetsPath + "/data.json";

        var jsonString = File.ReadAllText(path);
        levelData = LevelController.JsonHelper.FromJson<LevelController.levelStruct>(jsonString);

        Transform parent = GameObject.Find("Sublevels").transform;
        foreach (Transform child in parent)
        {
            sublevelList.Add(child.gameObject);
        }
        parent = GameObject.Find("Levels").transform;
        foreach (Transform child in parent)
        {
            levelList.Add(child.gameObject);
        }
        sublevelList[0].GetComponent<Image>().sprite = activeSublevelspr;
        levelList[0].GetComponent<Image>().sprite = activeLevelspr;
        LoadImage(0);
    }

    public void PressedLevelButton()
    {
        GameObject button = EventSystem.current.currentSelectedGameObject;
        int i = int.Parse(button.name);
        i -= 1;
        SwitchingLevel(i, button.tag);        
    }

    private void SwitchingLevel(int i, string tag)
    {
        sublevelList[activeSublevel].GetComponent<Image>().sprite = unactiveSublevelspr;
        levelList[activeLevel].GetComponent<Image>().sprite = unactiveLevelspr;

        if (i < 0)
        {
            i = 0;
        }

        if (tag == "Level")
        {
            if (i > 7)
            {
                i = 7;
            }

            if (i != 0 && !levelData[(i-1)*5+4].passed)
            {
                i = activeLevel;
            }

            levelList[i].GetComponent<Image>().sprite = activeLevelspr;
            sublevelList[0].GetComponent<Image>().sprite = activeSublevelspr;
            activeLevel = i;
            activeSublevel = 0;
            i = i*5;
        }

        if (tag == "Sublevel")
        {
            if (i > 4)
            {
                i = 4;
            }
            levelList[activeLevel].GetComponent<Image>().sprite = activeLevelspr;
            sublevelList[i].GetComponent<Image>().sprite = activeSublevelspr;
            activeSublevel = i;
            i = activeLevel*5 + i;
        }

        LoadImage(i);
    }

    private void LoadImage(int index)
    {
        GameObject image = GameObject.Find("Image");
        Texture2D texture = Resources.Load<Texture2D>(levelData[index].imagePath);
        texture = Slicer.Resize(texture);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f, 100f);
        image.GetComponent<Image>().sprite = sprite;
        image.GetComponent<RectTransform>().sizeDelta = new Vector2(texture.width, texture.height);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SwitchingLevel(activeSublevel-1, "Sublevel");
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SwitchingLevel(activeSublevel+1, "Sublevel");
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SwitchingLevel(activeLevel-1, "Level");
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SwitchingLevel(activeLevel+1, "Level");
        }        
    }
}

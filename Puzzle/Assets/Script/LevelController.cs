using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    [Serializable]
    public class levelStruct
    {
        public int level;
        public int sublevel;
        public int size;
        public string imagePath;
        public bool passed;
    }
    public static List<Vector3> slicesPosToWin = new List<Vector3>();
    public static List<Vector3> slicesPos = new List<Vector3>();
    public static List<GameObject> slicesList = new List<GameObject>();
    public static float speed;
    public static GameObject slicePref;
    public static int currentLevelIndex = 0;
    public static Texture2D img;
    public static string path;
    private const int sublevelNumber = 5;
    public static bool anim = false;
    public static GameObject[] outlineList;

    void Start()
    {
        slicePref = Resources.Load<GameObject>("Prefab/SlicePref");
        path = Application.streamingAssetsPath + "/data.json";

        var jsonString = File.ReadAllText(path);
        levelStruct[] levelData = JsonHelper.FromJson<levelStruct>(jsonString);

        string json = JsonHelper.ToJson(levelData, true);
        File.WriteAllText(path, json);

        var i = LoadLevelData(currentLevelIndex);
    }
    public static int LoadLevelData(int index)
    {
        Swap.win = false;

        var jsonString = File.ReadAllText(path);
        levelStruct[] levelData = JsonHelper.FromJson<levelStruct>(jsonString);

        if (index < 0)
        {
            index = 0;
        }
        if (index > levelData.Length-1)
        {
            ButtonController.FinalLevel();
            return currentLevelIndex;
        }
        int i = index - index%5;
        if ((i != 0 && levelData[i-1].passed) || (index < 5))
        {
            var lastLevelIndex = currentLevelIndex;
            currentLevelIndex = index;

            speed = 6f * Screen.height / 10f; 
            img = Resources.Load<Texture2D>(levelData[currentLevelIndex].imagePath);
            Vector2Int size = new Vector2Int(levelData[currentLevelIndex].size, levelData[currentLevelIndex].size);
            Slicer.Slice(img, size, slicePref);
            slicesPos = Slicer.MixSlices(slicesPos, slicesPosToWin, slicesList);
            outlineList = GameObject.FindGameObjectsWithTag("Outline");

            return lastLevelIndex;
        }
        else
        {
            LoadLevelData(currentLevelIndex);
            return currentLevelIndex;
        }
    }

    public static void CleanLevelData()
    {
        slicesPosToWin = new List<Vector3>();
        slicesPos = new List<Vector3>();
        slicesList = new List<GameObject>();

        if (Swap.win)
        {
            var jsonString = File.ReadAllText(path);
            levelStruct[] levelData = JsonHelper.FromJson<levelStruct>(jsonString);
            levelData[currentLevelIndex].passed = true;
            string json = JsonHelper.ToJson(levelData, true);
            File.WriteAllText(path, json);
        }
    }
    public static void AutoComplete()
    {
        for (int i = 0; i < slicesList.Count; i++)
        {
            slicesList[i].GetComponent<RectTransform>().localPosition = slicesPosToWin[i];
        }
    }

    void Update()
    {
        if (Swap.win)
        {
            foreach (GameObject go in outlineList)
            {
                Color lastCol = go.GetComponent<Image>().color;
                go.GetComponent<Image>().color = Color.Lerp(lastCol, new Color(1f,1f,1f,0f), Time.deltaTime*5f);
            }
        }
    }

    public static void CleanPieces()
    {
        CleanChild("Shadow");
        CleanChild("Outline");
        CleanChild("ImagePiece");
    }

    public static void CleanChild(string tag)
    {
        GameObject[] allChildren = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject child in allChildren)
        {
            DestroyImmediate(child.gameObject);
        }
    }

    public class JsonHelper 
    {
        public static T[] FromJson<T>(string json) 
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array) 
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint) 
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T> 
        {
            public T[] Items;
        }
    }
}

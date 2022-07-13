using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slicer : MonoBehaviour
{
    public static Sprite outlineSprite;
    public static void Slice(Texture2D img, Vector2Int size, GameObject slicePref)
    {
        img = Resize(img);

        var parent = GameObject.Find("Gameplay").transform;
        var outlinePref = Resources.Load<GameObject>("Prefab/outlinePref");

        Vector2Int tileSize = new Vector2Int (Mathf.FloorToInt(img.width/(float)size.x), Mathf.FloorToInt(img.height/(float)size.y));

        for (var y = size.y - 1; y >= 0; y--)
        {
            for (var x = 0; x < size.x; x++)
            {
                var bottomLeftPixelX = x * tileSize.x;
                var bottomLeftPixelY = y * tileSize.y;

                var sprite = Sprite.Create(img, new Rect(bottomLeftPixelX, bottomLeftPixelY, tileSize.x, tileSize.y), Vector2.one * 0.5f, 100f);

                GameObject go = Instantiate(slicePref, new Vector3(0f, 0f, 0f), Quaternion.identity);
                var rect = go.GetComponent<RectTransform>();
                SetParentAndReset(rect, parent);

                go.GetComponent<Image>().sprite = sprite;
                rect.sizeDelta = new Vector2(tileSize.x, tileSize.y);
                rect.localPosition = new Vector3(bottomLeftPixelX-(float)size.x/2*tileSize.x+tileSize.x/2, bottomLeftPixelY-(float)size.x/2*tileSize.y+tileSize.y/2, 0.5f);

                GameObject outline = Instantiate(outlinePref, new Vector3(0f, 0f, 0f), Quaternion.identity);
                var outlineRect = outline.GetComponent<RectTransform>();
                SetParentAndReset(outlineRect, go.transform);
                outlineRect.sizeDelta = new Vector2(tileSize.x, tileSize.y);

                LevelController.slicesPosToWin.Add(rect.localPosition);
                LevelController.slicesPos.Add(rect.localPosition);
                LevelController.slicesList.Add(go);
            }
        }
    }
    public static void SetParentAndReset(RectTransform rect, Transform parent) 
    {
        rect.SetParent(parent);
        rect.localScale = Vector3.one;
        rect.localPosition = Vector3.zero;
    }

    public static List<Vector3> MixSlices(List<Vector3> slicesPos, List<Vector3> slicesPosToWin, List<GameObject> slicesList)
    {
        for (var i = 0; i < slicesPosToWin.Count/2; i++)
        {
            var randomSliceIndex = Random.Range(0, slicesPosToWin.Count);
            Vector3 sliceStore = slicesPos[i];
            slicesPos[i] = slicesPos[randomSliceIndex];
            slicesPos[randomSliceIndex] = sliceStore;
            slicesList[i].GetComponent<RectTransform>().localPosition = slicesPos[i];
            slicesList[randomSliceIndex].GetComponent<RectTransform>().localPosition = slicesPos[randomSliceIndex];
        }

        return slicesPos;
    }
    public static Texture2D Resize(Texture2D texture2D)
    {
        var ScreenSize = new Vector2(Screen.width, Screen.height);
        float diff = (float)texture2D.width/(float)texture2D.height;
        int targetY = Mathf.FloorToInt(ScreenSize.y*0.8f);
        int targetX = Mathf.FloorToInt(diff*targetY);
        if (targetX >= Mathf.FloorToInt(ScreenSize.y))
        {
            targetX = targetY;
        }
        RenderTexture rt=new RenderTexture(targetX, targetY, 24);
        RenderTexture.active = rt;
        Graphics.Blit(texture2D,rt);
        Texture2D result=new Texture2D(targetX,targetY);
        result.ReadPixels(new Rect(0,0,targetX,targetY),0,0);
        result.Apply();
        return result;
    }
}

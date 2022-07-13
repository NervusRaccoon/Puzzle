using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Swap : MonoBehaviour
{
    private GameObject firstChoice = null;
    private GameObject secondChoice = null;
    private Vector3 firstPos;
    private Vector3 secondPos;
    public static bool win = false;
    private bool ifWin;
    private bool swapAnim = false;
    private Color selected = new Color(0.7f, 0.7f, 0.7f, 1f);
    private Color normal = new Color(1f, 1f, 1f, 1f);

    void Update()
    {
        if (!win)
        {
            if (!swapAnim)
            {
                if (Input.GetMouseButtonDown(0))
                { 
                    PointerEventData pointer = new PointerEventData(EventSystem.current);
                    pointer.position = Input.mousePosition;

                    List<RaycastResult> raycastResults = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(pointer, raycastResults);
                    if (raycastResults.Count > 0)
                    {
                        foreach(var transf in raycastResults)
                        {
                            GameObject go = transf.gameObject;
                            if (go.tag == "ImagePiece")
                            {
                                if (firstChoice == null)
                                {
                                    firstChoice = go;
                                    firstPos = go.GetComponent<RectTransform>().localPosition;
                                    firstChoice.GetComponent<Image>().color = selected;
                                }
                                else
                                {
                                    Vector3 dist = firstChoice.transform.position - go.transform.position;
                                    if ((Mathf.Abs(dist.x) <= go.GetComponent<RectTransform>().sizeDelta.x && dist.y == 0)
                                            || (Mathf.Abs(dist.y) <= go.GetComponent<RectTransform>().sizeDelta.y && dist.x == 0)
                                            && go != firstChoice)
                                    {
                                        secondChoice = go;
                                        secondPos = go.GetComponent<RectTransform>().localPosition;
                                        int indexGO = LevelController.slicesPos.IndexOf(secondPos);
                                        int indexChosen = LevelController.slicesPos.IndexOf(firstPos);
                                        LevelController.slicesPos[indexGO] = firstPos;
                                        LevelController.slicesPos[indexChosen] = secondPos;
                                        ifWin = true;
                                        for (var i = 0; i < LevelController.slicesPosToWin.Count; i++)
                                        {
                                            if (LevelController.slicesPos[i] != LevelController.slicesPosToWin[i])
                                            {
                                                ifWin = false;
                                            }
                                        }
                                        firstChoice.GetComponent<Image>().color = normal;    
                                        swapAnim = true;
                                        SwapAnimation();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                SwapAnimation();
            }
        }
        else
        {
            if (swapAnim)
            {
                swapAnim = false;
                secondChoice.GetComponent<RectTransform>().localPosition = firstPos;
                firstChoice.GetComponent<RectTransform>().localPosition = secondPos;
            }
            if (firstChoice != null && firstChoice.GetComponent<Image>().color == selected)
            {
                firstChoice.GetComponent<Image>().color = normal;
            }
        }
    }

    private void SwapAnimation()
    {
        if (firstChoice.GetComponent<RectTransform>().localPosition == secondPos && secondChoice.GetComponent<RectTransform>().localPosition == firstPos)
        {
            if (!ifWin)
            {
                swapAnim = false;
                firstChoice = null;
                secondChoice = null;
            }
            else
            {
                swapAnim = false;
                win = true;
            }
        }
        else
        {
            secondChoice.GetComponent<RectTransform>().localPosition= Vector3.MoveTowards(secondChoice.GetComponent<RectTransform>().localPosition, firstPos, Time.deltaTime * LevelController.speed);
            firstChoice.GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(firstChoice.GetComponent<RectTransform>().localPosition, secondPos, Time.deltaTime * LevelController.speed);
        }
    }
}

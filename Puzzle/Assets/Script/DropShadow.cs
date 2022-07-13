using UnityEngine;
using UnityEngine.UI;

public class DropShadow : MonoBehaviour
{
    public GameObject shadowPref;
    public Vector2 ShadowOffset;
    private GameObject shadow;
    private Color normal = new Color(1f,1f,1f,0.1f);
    private Color selected = new Color(1f, 1f, 1f, 0.2f);
    private Color normalSlice = new Color(1f,1f,1f,1f);

    void Start()
    {
        var parent = GameObject.Find("Shadows").transform;
        shadow = Instantiate(shadowPref, new Vector3(0f, 0f, 0f), Quaternion.identity);
        var shadowRect = shadow.GetComponent<RectTransform>();
        Slicer.SetParentAndReset(shadowRect, parent);
        shadowRect.sizeDelta = gameObject.GetComponent<RectTransform>().sizeDelta;
    }

    void LateUpdate()
    {
        shadow.transform.localPosition = transform.localPosition + (Vector3)ShadowOffset;
        if (gameObject.GetComponent<Image>().color != normalSlice)
        {
            shadow.GetComponent<Image>().color = selected;
        }
        else
        {
            shadow.GetComponent<Image>().color = normal;
        }
    }
}
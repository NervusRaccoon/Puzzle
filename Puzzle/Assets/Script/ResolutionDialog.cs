using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionDialog : MonoBehaviour
{
    Resolution[] resolutions;
    private Dropdown dropdownMenu;
    void Start()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        dropdownMenu = GameObject.Find("Dropdown").GetComponent<Dropdown>();
        resolutions = Screen.resolutions.Select
                (resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();
        for (int i = 0; i < resolutions.Length; i++)
        {
            dropdownMenu.options.Add(new Dropdown.OptionData(ResToString(resolutions[i])));
            dropdownMenu.options[i].text = ResToString(resolutions[i]);
            dropdownMenu.value = i;
        }

        dropdownMenu.options.Add(new Dropdown.OptionData("Full Screen"));
        dropdownMenu.options[resolutions.Length].text = "Full Screen";
        dropdownMenu.value = resolutions.Length;
        //Screen.SetResolution(resolutions[resolutions.Length-1].width, resolutions[resolutions.Length-1].height, false);

        dropdownMenu.onValueChanged.AddListener(delegate{OnClick(dropdownMenu);});
    }

    private void OnClick(Dropdown resolution)
    {
        if (resolution.value != resolutions.Length)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            Screen.SetResolution(resolutions[resolution.value].width, resolutions[resolution.value].height, false);
        }
        else
        {
            Screen.SetResolution(resolutions[resolutions.Length-1].width, resolutions[resolutions.Length-1].height, false);
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
    }
 
    string ResToString(Resolution res)
    {
        return res.width + " x " + res.height;
    }
}

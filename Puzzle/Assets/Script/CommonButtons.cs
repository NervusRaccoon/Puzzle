using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CommonButtons : MonoBehaviour
{
    private GameObject menuPanel;
    private bool menuOn = false;
    private Sprite mutedAudioSpr;
    private Sprite audioSpr;
    private GameObject audioButton;

    void Start()
    {
        audioButton = GameObject.Find("SoundButton");
        menuPanel = GameObject.Find("MenuOnPanel");
        menuPanel.SetActive(false);
        audioSpr = Resources.Load<Sprite>("Sprite/Interface/sound_on_button");
        mutedAudioSpr = Resources.Load<Sprite>("Sprite/Interface/sound_off_button");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            PressOnExitButton(); 
        }
    }

    public void PressOnExitButton()
    {
        Application.Quit(); 
    }
    public void PressOnAudioButton()
    {
        AudioSource audio = GameObject.Find("Audio").GetComponent<AudioSource>();
        if (!audio.mute)
        {
            audioButton.GetComponent<Image>().sprite = mutedAudioSpr;
            AudioController.MuteAudio(true);
        }
        else
        {
            audioButton.GetComponent<Image>().sprite = audioSpr;
            AudioController.MuteAudio(false);            
        }
    }

    public void PressGalleryButton()
    {
        ButtonController.CleanPrevLevel();
        SceneManager.LoadScene("Gallery", LoadSceneMode.Single);
    }
    public void PressLevelMenuButton()
    {
        ButtonController.CleanPrevLevel();
        SceneManager.LoadScene("LevelMenu", LoadSceneMode.Single);
    }
    public void PressOnMenuButton()
    {
        if (!menuOn)
        {
            menuPanel.SetActive(true);
            menuOn = true;
        }
        else
        {
            menuPanel.SetActive(false);
            menuOn = false;
        }
    }
}

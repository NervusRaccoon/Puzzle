using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AudioController : MonoBehaviour
{
    public static AudioSource audio;
    private AudioClip firstSound;
    private AudioClip secondSound;
    private static GameObject playerInstance = null;
    void Awake()
    {
        if (playerInstance == null) 
        {
            playerInstance = gameObject;
            audio = playerInstance.GetComponent<AudioSource>();
            firstSound = Resources.Load<AudioClip>("Audio/sound1");
            secondSound = Resources.Load<AudioClip>("Audio/sound2");
            audio.clip = secondSound;
            DontDestroyOnLoad(gameObject);
        } 
    }
    void LateUpdate()
    {
        if (!audio.isPlaying && Application.isFocused)
        {
            if (audio.clip == firstSound)
            {
                audio.clip = secondSound;
            }
            else
            {
                audio.clip = firstSound;
            }
            audio.Play();
        }
    }

    public static void MuteAudio(bool mute)
    {
        audio.mute = mute;
    }
    void OnApplicationFocus(bool focus)
    {
        
    }
}

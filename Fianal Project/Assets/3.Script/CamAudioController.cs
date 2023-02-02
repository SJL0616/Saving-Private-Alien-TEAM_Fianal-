using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamAudioController : MonoBehaviour
{
    public AudioClip audioStart;
    public AudioClip audioCombat;
    public AudioClip audioVictory;
    public AudioClip audioDefeat;
    public int currentBgm = 1;

    AudioSource audioSource;
    public void PlaySound(string action){
        switch(action){
            case "Start":
                audioSource.clip = audioStart;
                audioSource.volume = 0.1f;
                break;
            case "Combat":
                audioSource.clip = audioCombat;
                audioSource.loop = true;
                audioSource.volume = 0.1f;
                break;
            case "Victory":
                audioSource.clip = audioVictory;
                audioSource.loop = false;
                audioSource.volume = 0.1f;
                break;
            case "Defeat":
                audioSource.clip = audioDefeat;
                audioSource.loop = false;
                audioSource.volume = 0.1f;
                break;          
        }
        audioSource.Play();
    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlaySound("Start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

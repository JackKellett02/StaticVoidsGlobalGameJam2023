using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSoundManager : MonoBehaviour
{
    [SerializeField]
    private List<AudioSource> audioList = new List<AudioSource>();

    [SerializeField]
    private Slider volumeKnob;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AudioListener.volume = volumeKnob.value;
    }

    public void PlaySound(int listNumber)
    {
        audioList[listNumber].pitch = Random.Range(1.0f, 1.5f);
        audioList[listNumber].Play();
    }
}

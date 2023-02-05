using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    #region variables to assign via the unity inspector.
    [SerializeField]
    private float timeForFade = 1.0f;

    [SerializeField]
    private AudioSource chillMusic = null;

    [SerializeField]
    private AudioSource chaoticMusic = null;
    #endregion

    #region private variables.

    #endregion

    #region private functions.
    // Start is called before the first frame update
    void Start()
    {
        if(chillMusic == null)
        {
            Debug.LogError("Chill Music Audio Source was not assigned");
            return;
        }

        if (chaoticMusic == null)
        {
            Debug.LogError("Chill Music Audio Source was not assigned");
            return;
        }

        //Play the music.
        chillMusic.Play();
        chillMusic.loop = true;
        chaoticMusic.Play();
        chaoticMusic.loop = true;

        //Set music volumes.
        chillMusic.volume = 1.0f;
        chaoticMusic.volume = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    #region public access functions.
    public IEnumerator StartRoundMusic(bool RoundStarted)
    {
        //Get the correct target volumes.
        float chillTargetVolume = 1.0f;
        float chillCurrent = 0.0f;
        float chaoticTargetVolume = 0.0f;
        float chaoticCurrent = 1.0f;
        if (RoundStarted)
        {
            chaoticTargetVolume = 1.0f;
            chaoticCurrent = 0.0f;
            chillTargetVolume = 0.0f;
            chillCurrent = 1.0f;
        }

        float deltaTime = 0.0f;
        float timeLastFrame = 0.0f;
        float timer = 0.0f;
        while (timer <= timeForFade)
        {
            timeLastFrame = Time.time;
            yield return null;
            deltaTime = Time.time - timeLastFrame;
            timer += deltaTime;
            float control = Mathf.InverseLerp(0.0f, timeForFade, timer);

            //Set the volumes.
            chillMusic.volume = Mathf.Lerp(chillCurrent, chillTargetVolume, control);
            chaoticMusic.volume = Mathf.Lerp(chaoticCurrent, chaoticTargetVolume, control);
        }
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManagerScript : MonoBehaviour
{
    #region Variables to assign via the unity inspector.
    [SerializeField]
    [Range(0, 120)]
    private int timeBetweenWaves = 60;

    [SerializeField]
    private float resourcesPerAntKilled = 2.0f;

    [SerializeField]
    private float startingMoney = 10.0f;

    [SerializeField]
    private float spawnRateMult = 2.0f;

    [SerializeField]
    private AnimationCurve spawnRateCurve;
    #endregion

    #region Private Variables.
    private static float moneyCount;
    private int currentRound = 1;

    private static int timeBeforeNextRound = 0;
    #endregion

    #region Events
    public static EventHandler roundStarting;
    #endregion

    #region Private Functions.
    // Start is called before the first frame update
    void Start()
    {
        //Subscribe to events.
        AntAIScript.antKilled += OnAntKilled;
        AntSpawnerScript.roundOver += OnRoundOver;

        //Set up variables.
        timeBeforeNextRound = 10;
        moneyCount = 0;
        currentRound = 1;

        //Start timer.
        StartCoroutine(NextWaveCoolTimer());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDisable()
    {
        AntAIScript.antKilled -= OnAntKilled;
        AntSpawnerScript.roundOver -= OnRoundOver;
    }

    private void OnValidate()
    {
        if(startingMoney < 0.0f)
        {
            startingMoney = 0.0f;
        }
    }

    private AntSpawnerScript.Wave GenerateWaveFromRoundNumber(int a_round)
    {
        AntSpawnerScript.Wave wave;
        wave.numAntsToSpawn = a_round * 5;

        float inverseRound = a_round / int.MaxValue;
        wave.spawnSpeed = spawnRateCurve.Evaluate(inverseRound) * spawnRateMult;
        wave.numAntsToSpawn = int.MaxValue;
        wave.spawnSpeed = 0.1f;
        return wave;
    }

    private IEnumerator NextWaveCoolTimer()
    {
        while(timeBeforeNextRound > 0)
        {
            //Debug.Log(timeBeforeNextRound + " seconds left.");
            yield return new WaitForSeconds(1.0f);
            timeBeforeNextRound--;
        }

        roundStarting?.Invoke(this, EventArgs.Empty);
        AntSpawnerScript.StartWave(GenerateWaveFromRoundNumber(currentRound));
    }
    #endregion

    #region Event Handling.
    private void OnAntKilled(object sender, AntAIScript.AntInfo e)
    {
        if (!e.m_killedByDefencePoint)
        {
            //Add points to resources.
            moneyCount += resourcesPerAntKilled;
        }
        else
        {
            Debug.Log("Ant killed by damaging defence point, not by a tower.");
        }
    }

    private void OnRoundOver(object sender, EventArgs e)
    {
        Debug.Log("Round " + currentRound + " over.");
        //Increment round counter.
        currentRound++;

        //Show upgrades UI.

        //Start Timer for next round.
        timeBeforeNextRound = timeBetweenWaves;
        StartCoroutine(NextWaveCoolTimer());
    }
    #endregion

    #region Public Access Functions.
    public static int GetSecondsBeforeNextRound()
    {
        return timeBeforeNextRound;
    }

    public static float GetMoneyCount()
    {
        return moneyCount;
    }


    public static void HandleGameOver()
    {
        Debug.Log("GAME OVER");
    }
    #endregion
}

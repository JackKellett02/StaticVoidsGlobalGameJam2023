using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

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

    [SerializeField]
    private AudioManagerScript audioManager = null;

    [SerializeField]
    private List<UIChangeInt> moneyStuff = new List<UIChangeInt>();

    [SerializeField]
    private SpringDynamics timerBar = null;

    [SerializeField]
    private UIBarScale timerBarScale = null;

    [SerializeField]
    private UIBarScale healthBar = null;

    [SerializeField]
    private MenuHandler menuFade = null;

    [SerializeField]
    private GameObject appleAOE = null;

    private static MenuHandler menuFader = null;
    #endregion

    #region Private Variables.
    private static float moneyCount;
    private static List<UIChangeInt> moneyStuff2;
    private int currentRound = 1;

    private static int timeBeforeNextRound = 0;

    private bool firstFrame = true;
    private static bool playGameOver = false;
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
        playGameOver = false;
        timeBeforeNextRound = (int)timeBetweenWaves;
        timerBarScale.currentBarValue = timeBeforeNextRound;
        moneyCount = startingMoney;
        moneyStuff2 = moneyStuff;
        moneyStuff[0].ChangeInt((int)moneyCount);
        moneyStuff[1].ChangeInt((int)moneyCount);
        currentRound = 1;
        timerBarScale.maxBarValue = timeBetweenWaves;
        menuFader = menuFade;
    }

    // Update is called once per frame
    void Update()
    {
        if (firstFrame)
        {
            healthBar.maxBarValue = DefencePointScript.GetSystemMaxHealth();
            healthBar.currentBarValue = DefencePointScript.GetSystemCurrentHealth();
            firstFrame = false;
            //Start timer.
            StartCoroutine(NextWaveCoolTimer());
        }

        healthBar.currentBarValue = DefencePointScript.GetSystemCurrentHealth();

        if (playGameOver)
        {
            playGameOver = false;
            StartCoroutine(ReloadScene());
        }


        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10.0f);
            Instantiate(appleAOE, mousePos, Quaternion.identity, this.transform);
        }
    }

    private void OnDisable()
    {
        AntAIScript.antKilled -= OnAntKilled;
        AntSpawnerScript.roundOver -= OnRoundOver;
    }

    private void OnValidate()
    {
        if (startingMoney < 0.0f)
        {
            startingMoney = 0.0f;
        }
    }

    private AntSpawnerScript.Wave GenerateWaveFromRoundNumber(int a_round)
    {
        AntSpawnerScript.Wave wave;
        wave.numAntsToSpawn = a_round * 5;

        float inverseRound = a_round / 100;
        wave.spawnSpeed = spawnRateCurve.Evaluate(inverseRound) * spawnRateMult;
        //wave.numAntsToSpawn = int.MaxValue;
        //wave.spawnSpeed = 0.1f;
        return wave;
    }

    private IEnumerator NextWaveCoolTimer()
    {
        timerBar.SwitchPos();
        while (timeBeforeNextRound > 0)
        {
            //Debug.Log(timeBeforeNextRound + " seconds left.");
            yield return new WaitForSeconds(1.0f);
            timeBeforeNextRound--;
            timerBarScale.currentBarValue = timeBeforeNextRound;
        }

        //Play music.
        StartCoroutine(audioManager.StartRoundMusic(true));

        //Fire off round starting event.
        roundStarting?.Invoke(this, EventArgs.Empty);
        timerBar.SwitchPos();
        AntSpawnerScript.StartWave(GenerateWaveFromRoundNumber(currentRound));
    }

    private IEnumerator ReloadScene()
    {
        Debug.Log("STARTING RELOAD SCENE IN 3 SECS");
        yield return new WaitForSeconds(4.0f);
        SceneManager.LoadScene(0);
    }
    #endregion

    #region Event Handling.
    private void OnAntKilled(object sender, AntAIScript.AntInfo e)
    {
        if (!e.m_killedByDefencePoint)
        {
            //Add points to resources.
            moneyCount += resourcesPerAntKilled;
            moneyStuff[0].ChangeInt((int)moneyCount);
            moneyStuff[1].ChangeInt((int)moneyCount);
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

        //Switch music.
        StartCoroutine(audioManager.StartRoundMusic(false));

        //Show upgrades UI.

        //Start Timer for next round.
        timeBeforeNextRound = timeBetweenWaves;
        timerBarScale.currentBarValue = timeBeforeNextRound;
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

    public static void ReduceMoneyPlz(float money)
    {
        moneyCount = Mathf.Clamp(moneyCount - money, 0.0f, float.MaxValue);
        moneyStuff2[0].ChangeInt((int)moneyCount);
        moneyStuff2[1].ChangeInt((int)moneyCount);
    }

    public static void HandleGameOver()
    {
        Debug.Log("GAME OVER");
        //Put UI shit here.
        menuFader.screenFading = true;

        //Reload scene after 3 seconds.
        playGameOver = true;
    }
    #endregion
}

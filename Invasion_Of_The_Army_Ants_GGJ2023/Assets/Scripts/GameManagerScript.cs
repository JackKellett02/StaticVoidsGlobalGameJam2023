using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour {
	#region Variables to assign via the unity inspector.
	[SerializeField]
	[Range(0, 120)]
	private int timeBetweenWaves = 60;

	[SerializeField]
	private float resourcesPerAntKilled = 2.0f;

	[SerializeField]
	private float startingMoney = 10.0f;

	[Header("This multiplier makes the ants spawn more slowly when above 1.")]
	[SerializeField]
	private float spawnRateMult = 2.0f;

	[SerializeField]
	private int difficultyPlateuRound = 100;

	[Header("This multiplier is a constant value that will affect the number of ants spawned each round determined normally by the difficulty curve.")]
	[SerializeField]
	private int numAntsMultiplier = 5;

	[SerializeField]
	private AnimationCurve spawnRateCurve;

	[SerializeField]
	private AudioManagerScript audioManager = null;

	[SerializeField]
	private List<UIChangeInt> roundStuff = new List<UIChangeInt>();

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
	private static List<UIChangeInt> roundStuff2;
	private int currentRound = 1;

	private int numAnts = 0;
	private int numAntsLastFrame = 0;
	private static int timeBeforeNextRound = 0;

	private bool firstFrame = true;
	private static bool playGameOver = false;
	#endregion

	#region Events
	public static EventHandler roundStarting;
	public static EventHandler roundOver;
	#endregion

	#region Private Functions.
	// Start is called before the first frame update
	void Start() {
		//Subscribe to events.
		AntAIScript.antKilled += OnAntKilled;
		AntAIScript.antSpawned += OnAntSpawned;
		GameManagerScript.roundOver += OnRoundOver;

		//Set up variables.
		playGameOver = false;
		timeBeforeNextRound = (int)timeBetweenWaves;
		timerBarScale.currentBarValue = timeBeforeNextRound;
		moneyCount = startingMoney;
		roundStuff2 = roundStuff;
		//moneyStuff[0].ChangeInt((int)moneyCount);
		//moneyStuff[1].ChangeInt((int)moneyCount);
		currentRound = 0;
		roundStuff[0].ChangeInt((int)currentRound);
		roundStuff[1].ChangeInt((int)currentRound);
		timerBarScale.maxBarValue = timeBetweenWaves;
		menuFader = menuFade;
	}

	// Update is called once per frame
	void Update() {
		if (firstFrame) {
			healthBar.maxBarValue = DefencePointScript.GetSystemMaxHealth();
			healthBar.currentBarValue = DefencePointScript.GetSystemCurrentHealth();
			firstFrame = false;
			//Start timer.
			StartCoroutine(NextWaveCoolTimer());
		}

		healthBar.currentBarValue = DefencePointScript.GetSystemCurrentHealth();

		if (playGameOver) {
			playGameOver = false;
			StartCoroutine(ReloadScene());
		}


		if (Input.GetMouseButtonDown(1)) {
			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10.0f);
			Instantiate(appleAOE, mousePos, Quaternion.identity, this.transform);
		}

		//Check if the round is over.
		if (numAntsLastFrame == numAnts) {
			return;
		} else {
			numAntsLastFrame = numAnts;
			if (numAnts <= 0) {
				roundOver?.Invoke(this, EventArgs.Empty);
			}
		}

	}

	private void OnDisable() {
		AntAIScript.antKilled -= OnAntKilled;
		GameManagerScript.roundOver -= OnRoundOver;
		AntAIScript.antSpawned -= OnAntSpawned;
	}

	private void OnValidate() {
		if (startingMoney < 0.0f) {
			startingMoney = 0.0f;
		}
	}

	private AntSpawnerScript.Wave GenerateWaveFromRoundNumber(int a_round) {
		AntSpawnerScript.Wave wave;
		float inverseRound = Mathf.Clamp01((float)a_round / (float)difficultyPlateuRound);
		float difficultyCurve = spawnRateCurve.Evaluate(inverseRound);
		wave.spawnSpeed = difficultyCurve * spawnRateMult;
		wave.numAntsToSpawn = a_round * (int)((1 / difficultyCurve) * numAntsMultiplier);
		//wave.numAntsToSpawn = int.MaxValue;
		//wave.spawnSpeed = 0.1f;
		return wave;
	}

	private IEnumerator NextWaveCoolTimer() {
		timerBar.SwitchPos();
		while (timeBeforeNextRound > 0) {
			//Debug.Log(timeBeforeNextRound + " seconds left.");
			yield return new WaitForSeconds(1.0f);
			timeBeforeNextRound--;
			timerBarScale.currentBarValue = timeBeforeNextRound;
		}

		//Play music.
		StartCoroutine(audioManager.StartRoundMusic(true));

		//Increment round counter.
		currentRound++;
		roundStuff[0].ChangeInt((int)currentRound);
		roundStuff[1].ChangeInt((int)currentRound);

		//Fire off round starting event.
		roundStarting?.Invoke(this, EventArgs.Empty);
		timerBar.SwitchPos();
		AntSpawnerScript.StartWave(GenerateWaveFromRoundNumber(currentRound));
	}

	private IEnumerator ReloadScene() {
		Debug.Log("STARTING RELOAD SCENE IN 3 SECS");
		yield return new WaitForSeconds(4.0f);
		SceneManager.LoadScene(0);
	}
	#endregion

	#region Event Handling.
	private void OnAntKilled(object sender, AntAIScript.AntInfo e) {
		//Manage ant numbers.
		numAnts--;

		//Points stuff.
		if (!e.m_killedByDefencePoint) {
			//Add points to resources.
			moneyCount += resourcesPerAntKilled;
			//moneyStuff[0].ChangeInt((int)moneyCount);
			//moneyStuff[1].ChangeInt((int)moneyCount);
		} else {
			Debug.Log("Ant killed by damaging defence point, not by a tower.");
		}
	}

	private void OnAntSpawned(object sender, EventArgs e) {
		numAnts++;
	}

	private void OnRoundOver(object sender, EventArgs e) {
		//Debug.Log("Round " + currentRound + " over.");

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
	public static int GetSecondsBeforeNextRound() {
		return timeBeforeNextRound;
	}

	public static float GetMoneyCount() {
		return moneyCount;
	}

	public static void ReduceMoneyPlz(float money) {
		moneyCount = Mathf.Clamp(moneyCount - money, 0.0f, float.MaxValue);
		//moneyStuff2[0].ChangeInt((int)moneyCount);
		//moneyStuff2[1].ChangeInt((int)moneyCount);
	}

	public static void HandleGameOver() {
		Debug.Log("GAME OVER");
		//Put UI shit here.
		menuFader.screenFading = true;

		//Reload scene after 3 seconds.
		playGameOver = true;
	}
	#endregion
}

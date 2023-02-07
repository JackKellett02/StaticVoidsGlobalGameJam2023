using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AntSpawnerScript : MonoBehaviour
{
    #region Variables to assign via the unity inspector.
    [SerializeField]
    private Transform antPrefab = null;

    [SerializeField]
    private List<Vector3> spawnPositions = new List<Vector3>();
    #endregion

    #region Private variable declarations.
    private static bool spawnAnt = false;
    private static Wave waveInfo;
    #endregion

    #region Events.
    public static EventHandler roundOver;
    #endregion

    #region Private Functions.
    // Start is called before the first frame update
    void Start()
    {
        spawnAnt = false;
        waveInfo.numAntsToSpawn = 0;
        waveInfo.spawnSpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnAnt)
        {
            if (waveInfo.numAntsToSpawn <= 0)
            {
                spawnAnt = false;
                roundOver?.Invoke(this, EventArgs.Empty);
                return;
            }
            StartCoroutine(SpawnAntsCooldown(waveInfo.spawnSpeed));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < spawnPositions.Count; i++)
        {
            Gizmos.DrawSphere(spawnPositions[i], 0.25f);
        }
    }

    private Vector3 ChoosePosition()
    {
        int randomIndex = UnityEngine.Random.Range(0, spawnPositions.Count);
        return spawnPositions[randomIndex];
    }
    #endregion

    #region Timers.
    private IEnumerator SpawnAntsCooldown(float a_time)
    {
        Instantiate(antPrefab, ChoosePosition(), Quaternion.identity, this.transform);
        spawnAnt = false;
        waveInfo.numAntsToSpawn--;
        yield return new WaitForSeconds(a_time);
        spawnAnt = true;
    }
    #endregion

    #region Public Access Functions.
    public static void StartWave(Wave a_waveInfo)
    {
        Debug.Log("Wave Started");
        waveInfo = a_waveInfo;
        //Debug.Log("Spawning " + waveInfo.numAntsToSpawn + " ants.");
        spawnAnt = true;
    }
    #endregion

    #region Structs.
    public struct Wave
    {
        public int numAntsToSpawn;
        public float spawnSpeed;
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //TODO: adjust the script to do not wait till the player kill all the minions before the next wave starts
    public enum SpawnState
    {
        SPAWNING,
        WAITING,
        COUNTING
    };

   [System.Serializable]
    public class Wave
    {
        public GameObject minionPrefab;
        [Tooltip("The number of minions will spawn.")]
        public int count;
        [Tooltip("The time between 2 minions spawned.")]
        public float rate;
    }
    public SpawnState state = SpawnState.COUNTING;
    public Wave[] waves;
    private int nextWave;

    public Transform enemySpawnPoint;
    public Transform allieSpawnPoint;
    public float timeBetweenWaves = 10f;
    public float waveCountDown;

    private float searchCountDown = 1f;

    private void Start()
    {
        waveCountDown = timeBetweenWaves;
    }

    private void Update()
    {
        if(state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive())
            {
                WaveCompleted();
            }
            else
            {
                return;
            }
        }


        if(waveCountDown <= 0)
        {
            if(state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountDown -= Time.deltaTime;
        }
    }
    void WaveCompleted()
    {
        state = SpawnState.COUNTING;

        waveCountDown = timeBetweenWaves;

      /*  if(nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
        }

        nextWave++;*/
    }
    bool EnemyIsAlive()
    {
        searchCountDown -= Time.deltaTime;
        if(searchCountDown <= 0)
        {
            searchCountDown = 1f;
            if(GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }
    IEnumerator SpawnWave(Wave wave)
    {
        state = SpawnState.SPAWNING;
        for(int i=0; i< wave.count; i++)
        {
            SpawnMinion(wave.minionPrefab);
            yield return new WaitForSeconds(1f / wave.rate);
        }
        
        state = SpawnState.WAITING;
        yield break;
    }

    void SpawnMinion(GameObject minion)
    {
        Instantiate(minion, enemySpawnPoint.position, Quaternion.identity);
       // Instantiate(minion, allieSpawnPoint.position, Quaternion.identity);
    }
}

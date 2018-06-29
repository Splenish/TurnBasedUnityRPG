using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public Transform[] spawnLocations;
    public GameObject[] whatToSpawnPrefab;
    public GameObject[] whatToSpawnClone;
    public EnemyStateMachine ESM;
    public BattleStateMachine BSM;
   

    void Awake()
    {

        spawnUnits();


    }

    void spawnUnits()
    {
      
        

        GameObject NewEnemy = Instantiate(whatToSpawnPrefab[0], spawnLocations[0].transform.position, Quaternion.Euler(0, 90, 0)) as GameObject;
       
        whatToSpawnClone[1] = Instantiate(whatToSpawnPrefab[1], spawnLocations[1].transform.position, Quaternion.Euler(0, 90, 0)) as GameObject;
        whatToSpawnClone[2] = Instantiate(whatToSpawnPrefab[2], spawnLocations[2].transform.position, Quaternion.Euler(0, 90, 0)) as GameObject;
        
        
    }
}
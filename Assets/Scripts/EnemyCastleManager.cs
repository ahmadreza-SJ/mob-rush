using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class EnemyCastleManager
{
    [SerializeField] [Range(10, 100)] private int health = 10;
    [SerializeField] [Range(0.1f, 10)] private float spawnInterval = 1f;
    [FormerlySerializedAs("enemyCastles")] [SerializeField] private List<EnemyCastleController> castles;
    
    
    public static Action<EnemyCastleController, Vector3> CastleSpawnTimeReached;

    public List<EnemyCastleController> Castles => castles;

    // Start is called before the first frame update
    public void Initialize()
    {
        foreach (EnemyCastleController enemyCastle in castles)
        {
            enemyCastle.Initialize(health, spawnInterval);
        }   
    }
}

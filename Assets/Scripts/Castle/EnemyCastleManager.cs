using System;
using System.Collections;
using System.Collections.Generic;
using Castle;
using Unity.VisualScripting;
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


    public bool TryGetNearestCastle(Vector3 to, out EnemyCastleController nearestCastle)
    {
        nearestCastle = null;
        if (castles.Count == 0)
        {
            return false;
        }
        
        float minDistance = float.MaxValue;
        foreach (EnemyCastleController castle in castles)
        {
            float dist = (to - castle.GetPosition()).magnitude;
            if (minDistance > dist)
            {
                minDistance = dist;
                nearestCastle = castle;
            }
            
        }

        return true;
    }
    // Start is called before the first frame update
    public void Initialize()
    {
        foreach (EnemyCastleController enemyCastle in castles)
        {
            enemyCastle.Initialize(health, spawnInterval);
        }   
    }
}

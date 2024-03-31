using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject shakerPrefab;
    [SerializeField]
    private GameObject orbiterPrefab;
    [SerializeField]
    private Transform player;

    public void SpawnRandom() {
        Vector2 pos = player.position.normalized * (17 + Random.value * 7);
        pos = Quaternion.AngleAxis(60 + Random.value * 240, Vector3.forward) * pos;

        if(Random.value > 0.5f) {
            //shaker
            Instantiate(shakerPrefab, pos, new Quaternion());
        }
        else {
            Instantiate(orbiterPrefab, pos, new Quaternion());
        }
    }
}

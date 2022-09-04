using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] GameObject smallEnemey;
    private bool _canSpawn = true;
    private float _spawnWait;

    // Start is called before the first frame update
    void Start()
    {
        if (smallEnemey == null)
        {
            Debug.LogError("Enemy not found on SpawnManager script.");
        }
        _spawnWait = Random.Range(1f, 5f);
        StartCoroutine(SpawnEnemies());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnEnemies()
    {
        while(_canSpawn)
        {
            Instantiate(smallEnemey);
            yield return new WaitForSeconds(_spawnWait);
        }
    }

    
}

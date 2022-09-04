using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] GameObject smallEnemey;
    private bool _canSpawn = true;
    private float _spawnWait;
    private GameObject _enemyParent;

    // Start is called before the first frame update
    void Start()
    {
        if (smallEnemey == null)
        {
            Debug.LogError("Enemy not found on SpawnManager script.");
        }
        if (_enemyParent == null)
        {
            _enemyParent = new GameObject("Enemies");
            _enemyParent.transform.SetParent(this.transform);
        }

        Spawn(_canSpawn);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Spawn(bool canSpawn)
    {
        _canSpawn = canSpawn;
        if (_canSpawn)
        {
            StartCoroutine(SpawnEnemies());
        }
    }

    private IEnumerator SpawnEnemies()
    {
        while (_canSpawn)
        {
            Instantiate(smallEnemey, _enemyParent.transform);
            _spawnWait = Random.Range(1f, 5f);
            yield return new WaitForSeconds(_spawnWait);
        }
    }


}

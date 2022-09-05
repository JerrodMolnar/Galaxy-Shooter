using System.Collections;
using UnityEngine;
using Utility;

namespace SpawnManager
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] private GameObject _smallEnemey;
        [SerializeField] private GameObject[] _powerups;
        private bool _canSpawn = true;
        private float _spawnWait;
        private GameObject _enemyParent;
        private GameObject _powerupParent;

        void Start()
        {
            if (_smallEnemey == null)
            {
                Debug.LogError("Enemy not found on SpawnManager script.");
            }
            if (_enemyParent == null)
            {
                _enemyParent = new GameObject("Enemies");
                _enemyParent.transform.SetParent(this.transform);
            }
            if (_powerupParent == null)
            {
                _powerupParent = new GameObject("Powerups");
                _powerupParent.transform.SetParent(this.transform);
            }
            if (_powerups == null)
            {
                Debug.LogError("Powerups are null on SpawnManager");
            }

            Spawn(_canSpawn);
        }

        public void Spawn(bool canSpawn)
        {
            _canSpawn = canSpawn;
            if (_canSpawn)
            {
                StartCoroutine(SpawnEnemies());
                StartCoroutine(SpawnPowerups());
            }
        }

        private IEnumerator SpawnEnemies()
        {
            while (_canSpawn)
            {
                Instantiate(_smallEnemey, _enemyParent.transform);
                _spawnWait = Random.Range(1f, 5f);
                yield return new WaitForSeconds(_spawnWait);
            }
        }

        private IEnumerator SpawnPowerups()
        {
            while (_canSpawn)
            {
                yield return new WaitForSeconds(Random.Range(5f, 10f));
                Vector3 posToSpawn = new Vector3(Random.Range(-(Helper.GetXPositionBounds()), 
                    Helper.GetXPositionBounds()), Helper.GetYUpperScreenBounds() + 2.5f, 0);
                Instantiate(_powerups[Random.Range(0, _powerups.Length)], posToSpawn, Quaternion.identity, _powerupParent.transform);
            }
        }
    }
}

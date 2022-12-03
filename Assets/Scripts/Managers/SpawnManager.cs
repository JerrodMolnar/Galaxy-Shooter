using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SpawnManager
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] private GameObject _smallEnemey;
        [SerializeField] private GameObject[] _powerups;
        private int _lastPowerup = -1;
        private List<GameObject> _speedPowerupsPool = new List<GameObject>();
        private List<GameObject> _tripleShotPowerupsPool = new List<GameObject>();
        private List<GameObject> _shieldPowerupPool = new List<GameObject>();
        private List<GameObject> _enemyPool = new List<GameObject>();
        private List<GameObject> _ammoPowerupPool = new List<GameObject>();
        private bool _canSpawn = false;
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
            yield return new WaitForSeconds(1f);
            while (_canSpawn)
            {
                bool isActiveEnemy = true;

                foreach (GameObject itemInPool in _enemyPool)
                {
                    if (itemInPool.activeSelf == false)
                    {
                        itemInPool.GetComponent<Enemy>().enabled = true;
                        itemInPool.SetActive(true);
                        itemInPool.GetComponent<PolygonCollider2D>().enabled = true;
                        isActiveEnemy = false;
                        break;
                    }
                }
                if (isActiveEnemy)
                {
                    GameObject newEnemy = Instantiate(_smallEnemey, _enemyParent.transform);
                    _enemyPool.Add(newEnemy);
                }
                _spawnWait = Random.Range(1f, 5f);
                yield return new WaitForSeconds(_spawnWait);
            }
        }

        private IEnumerator SpawnPowerups()
        {
            while (_canSpawn)
            {
                int randomIndexPowerup = Random.Range(0, _powerups.Length);
                while (randomIndexPowerup == _lastPowerup && _powerups.Length > 1)
                {
                    randomIndexPowerup = Random.Range(0, _powerups.Length);
                    yield return new WaitForSeconds(0.25f);
                }
                _lastPowerup = randomIndexPowerup;

                switch (_lastPowerup)
                {
                    case 0:
                        SpawnTripleShotPowerup();
                        break;
                    case 1:
                        SpawnSpeedPowerup();
                        break;
                    case 2:
                        SpawnShieldPowerup();
                        break;
                    case 3:
                        SpawnAmmoPowerup();
                        break;
                }
                yield return new WaitForSeconds(Random.Range(5f, 10f));
            }
        }

        private void SpawnTripleShotPowerup()
        {
            bool isActiveTripleShotPowerup = true;
            Vector3 posToSpawn = new Vector3(Random.Range(-(Helper.GetXPositionBounds()), Helper.GetXPositionBounds()), Helper.GetYUpperScreenBounds() + 2.5f, 0);

            foreach (GameObject itemInPool in _tripleShotPowerupsPool)
            {
                if (itemInPool.activeSelf == false)
                {
                    itemInPool.SetActive(true);
                    itemInPool.transform.position = posToSpawn;
                    isActiveTripleShotPowerup = false;
                    break;
                }
            }
            if (isActiveTripleShotPowerup)
            {
                GameObject powerup =
                Instantiate(_powerups[0], posToSpawn, Quaternion.identity, _powerupParent.transform);
                _tripleShotPowerupsPool.Add(powerup);
            }
        }

        private void SpawnSpeedPowerup()
        {
            bool isActiveSpeedPowerup = true;
            Vector3 posToSpawn = new Vector3(Random.Range(-(Helper.GetXPositionBounds()), Helper.GetXPositionBounds()), Helper.GetYUpperScreenBounds() + 2.5f, 0);

            foreach (GameObject itemInPool in _speedPowerupsPool)
            {
                if (itemInPool.activeSelf == false)
                {
                    itemInPool.SetActive(true);
                    itemInPool.transform.position = posToSpawn;
                    isActiveSpeedPowerup = false;
                    break;
                }
            }
            if (isActiveSpeedPowerup)
            {
                GameObject powerup = Instantiate(_powerups[1], posToSpawn, Quaternion.identity, _powerupParent.transform);
                _speedPowerupsPool.Add(powerup);
            }
        }

        private void SpawnShieldPowerup()
        {
            bool isActiveShieldPowerup = true;
            Vector3 posToSpawn = new Vector3(Random.Range(-(Helper.GetXPositionBounds()), Helper.GetXPositionBounds()), Helper.GetYUpperScreenBounds() + 2.5f, 0);

            foreach (GameObject itemInPool in _shieldPowerupPool)
            {
                if (itemInPool.activeSelf == false)
                {
                    itemInPool.SetActive(true);
                    itemInPool.transform.position = posToSpawn;
                    isActiveShieldPowerup = false;
                    break;
                }
            }
            if (isActiveShieldPowerup)
            {
                GameObject powerup = Instantiate(_powerups[2], posToSpawn, Quaternion.identity, _powerupParent.transform);
                _shieldPowerupPool.Add(powerup);
            }
        }

        private void SpawnAmmoPowerup()
        {
            bool isActiveAmmoPowerup = true;
            Vector3 posToSpawn = new Vector3(Random.Range(-(Helper.GetXPositionBounds()), Helper.GetXPositionBounds()), Helper.GetYUpperScreenBounds() + 2.5f, 0);

            foreach (GameObject itemInPool in _ammoPowerupPool)
            {
                if (itemInPool.activeSelf == false)
                {
                    itemInPool.SetActive(true);
                    itemInPool.transform.position = posToSpawn;
                    isActiveAmmoPowerup = false;
                    break;
                }
            }
            if (isActiveAmmoPowerup)
            {
                GameObject powerup = Instantiate(_powerups[3], posToSpawn, Quaternion.identity, _powerupParent.transform);
                _ammoPowerupPool.Add(powerup);
            }
        }
    }
}

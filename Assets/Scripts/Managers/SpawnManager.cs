using GameCanvas;
using ProjectileFire;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Utility;

namespace SpawnManager
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] _enemy;
        [SerializeField] private GameObject[] _powerups;
        private int _lastPowerup = -1;
        private int _secondLastPowerup = -1;
        private int _missileRareCount = 0;
        [SerializeField] private int _waveCount = 0;
        private int _enemySpawnCount = 0;
        private const int _MAX_ENEMIES_PER_WAVE = 5;
        private List<GameObject> _speedPowerupsPool = new List<GameObject>();
        private List<GameObject> _tripleShotPowerupsPool = new List<GameObject>();
        private List<GameObject> _shieldPowerupPool = new List<GameObject>();
        private List<GameObject> _enemySidewaysPool = new List<GameObject>();
        private List<GameObject> _enemyBossPool = new List<GameObject>();
        private List<GameObject> _enemyRegularPool = new List<GameObject>();
        private List<GameObject> _ammoPowerupPool = new List<GameObject>();
        private List<GameObject> _healthPowerupPool = new List<GameObject>();
        private List<GameObject> _missilePowerupPool = new List<GameObject>();
        private bool _canSpawn = false;
        private float _spawnWait;
        private GameObject _enemyParent;
        private GameObject _powerupParent;
        private GameObject _asteroid;
        private GameCanvasManager _gameCanvas;


        void Start()
        {
            if (_enemy == null)
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
            _asteroid = GameObject.Find("Asteroid");
            if (_asteroid == null)
            {
                Debug.LogError("Asteroid not found on SpawnManager");
            }
            _gameCanvas = GameObject.Find("Canvas").GetComponent<GameCanvasManager>(); 
            if (_gameCanvas == null)
            {
                Debug.LogError("Game Canvas not found on SpawnManager");
            }
        }

        public void StartSpawn(bool canSpawn)
        {
            _canSpawn = canSpawn;
            if (_canSpawn)
            {
                _waveCount++;
                _gameCanvas.UpdateWave(_waveCount);
                StartCoroutine(SpawnEnemies());
                StartCoroutine(SpawnPowerups());
            }
        }

        private IEnumerator SpawnEnemies()
        {
            yield return new WaitForSeconds(5f);
            bool canSpawnEnemies = _canSpawn;
            while (canSpawnEnemies)
            {
                yield return new WaitForSeconds(_spawnWait);
                int randomEnemy = Random.Range(0, _enemy.Length);

                if (_waveCount == 1 || randomEnemy == 0)
                {
                    SpawnRegularEnemy();
                    _enemySpawnCount++;
                }
                else if (randomEnemy == 1)
                {
                    SpawnRandomEnemy();
                    _enemySpawnCount++;
                }
                else if (_waveCount % 5 == 0 && randomEnemy == 2)
                {
                    _enemySpawnCount += 5;
                    SpawnBossEnemy();
                }
                if (_MAX_ENEMIES_PER_WAVE * _waveCount <= _enemySpawnCount)
                {
                    canSpawnEnemies = false;
                    _enemySpawnCount = 0;
                    IEnumerator WaitForNoEnemies()
                    {
                        while (Enemy.GetEnemyCount() > 0)
                        {
                            yield return new WaitForSeconds(0.5f);
                        }
                        StartSpawn(false);                        
                        _gameCanvas.EndWave(_waveCount);
                        yield return new WaitForSeconds(3f);
                        _asteroid.SetActive(true);
                    }
                    StartCoroutine(WaitForNoEnemies());
                    GameObject.FindGameObjectWithTag("Player").GetComponent<FireProjectiles>().AmmoPickup();
                }
            }
        }

        private void SpawnRegularEnemy()
        {
            bool isActiveEnemy = true;

            foreach (GameObject itemInPool in _enemyRegularPool)
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
                GameObject newEnemy = Instantiate(_enemy[0], _enemyParent.transform);
                _enemyRegularPool.Add(newEnemy);
            }
            _spawnWait = Random.Range(1f, 5f);
        }

        private void SpawnRandomEnemy()
        {
            bool isActiveEnemy = true;

            foreach (GameObject itemInPool in _enemySidewaysPool)
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
                GameObject newEnemy = Instantiate(_enemy[1], _enemyParent.transform);
                _enemySidewaysPool.Add(newEnemy);
            }
            _spawnWait = Random.Range(1f, 5f);
        }

        private void SpawnBossEnemy()
        {

        }

        private IEnumerator SpawnPowerups()
        {
            yield return new WaitForSeconds(5f);
            while (_canSpawn)
            {
                int randomIndexPowerup = Random.Range(0, _powerups.Length);

                while ((randomIndexPowerup == _lastPowerup || randomIndexPowerup == _secondLastPowerup) && _powerups.Length > 1)
                {
                    randomIndexPowerup = Random.Range(0, _powerups.Length);
                    yield return new WaitForSeconds(0.1f);
                }

                if (_lastPowerup != -1)
                {
                    _secondLastPowerup = _lastPowerup;
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
                    case 4:
                        SpawnHealthPowerup();
                        break;
                    case 5:
                        SpawnMissilePowerup();
                        break;
                }
                yield return new WaitForSeconds(Random.Range(5f, 10f));
            }
        }

        private void SpawnHealthPowerup()
        {
            bool noInactiveHealth = true;
            Vector3 posToSpawn = new Vector3(Random.Range(-(Helper.GetXPositionBounds()), Helper.GetXPositionBounds()), Helper.GetYUpperScreenBounds() + 2.5f, 0);

            foreach (GameObject itemInPool in _healthPowerupPool)
            {
                if (itemInPool.activeSelf == false)
                {
                    itemInPool.SetActive(true);
                    itemInPool.transform.position = posToSpawn;
                    noInactiveHealth = false;
                    break;
                }
            }
            if (noInactiveHealth)
            {
                GameObject powerup = Instantiate(_powerups[4], posToSpawn, Quaternion.identity, _powerupParent.transform);
                _healthPowerupPool.Add(powerup);
            }
        }

        private void SpawnTripleShotPowerup()
        {
            bool noInactiveTripleShot = true;
            Vector3 posToSpawn = new Vector3(Random.Range(-(Helper.GetXPositionBounds()), Helper.GetXPositionBounds()), Helper.GetYUpperScreenBounds() + 2.5f, 0);

            foreach (GameObject itemInPool in _tripleShotPowerupsPool)
            {
                if (itemInPool.activeSelf == false)
                {
                    itemInPool.SetActive(true);
                    itemInPool.transform.position = posToSpawn;
                    noInactiveTripleShot = false;
                    break;
                }
            }
            if (noInactiveTripleShot)
            {
                GameObject powerup =
                Instantiate(_powerups[0], posToSpawn, Quaternion.identity, _powerupParent.transform);
                _tripleShotPowerupsPool.Add(powerup);
            }
        }

        private void SpawnSpeedPowerup()
        {
            bool noInactiveSpeed = true;
            Vector3 posToSpawn = new Vector3(Random.Range(-(Helper.GetXPositionBounds()), Helper.GetXPositionBounds()), Helper.GetYUpperScreenBounds() + 2.5f, 0);

            foreach (GameObject itemInPool in _speedPowerupsPool)
            {
                if (itemInPool.activeSelf == false)
                {
                    itemInPool.SetActive(true);
                    itemInPool.transform.position = posToSpawn;
                    noInactiveSpeed = false;
                    break;
                }
            }
            if (noInactiveSpeed)
            {
                GameObject powerup = Instantiate(_powerups[1], posToSpawn, Quaternion.identity, _powerupParent.transform);
                _speedPowerupsPool.Add(powerup);
            }
        }

        private void SpawnShieldPowerup()
        {
            bool noInactiveShield = true;
            Vector3 posToSpawn = new Vector3(Random.Range(-(Helper.GetXPositionBounds()), Helper.GetXPositionBounds()), Helper.GetYUpperScreenBounds() + 2.5f, 0);

            foreach (GameObject itemInPool in _shieldPowerupPool)
            {
                if (itemInPool.activeSelf == false)
                {
                    itemInPool.SetActive(true);
                    itemInPool.transform.position = posToSpawn;
                    noInactiveShield = false;
                    break;
                }
            }
            if (noInactiveShield)
            {
                GameObject powerup = Instantiate(_powerups[2], posToSpawn, Quaternion.identity, _powerupParent.transform);
                _shieldPowerupPool.Add(powerup);
            }
        }

        private void SpawnAmmoPowerup()
        {
            bool noInactiveAmmo = true;
            Vector3 posToSpawn = new Vector3(Random.Range(-(Helper.GetXPositionBounds()), Helper.GetXPositionBounds()), Helper.GetYUpperScreenBounds() + 2.5f, 0);

            foreach (GameObject itemInPool in _ammoPowerupPool)
            {
                if (itemInPool.activeSelf == false)
                {
                    itemInPool.SetActive(true);
                    itemInPool.transform.position = posToSpawn;
                    noInactiveAmmo = false;
                    break;
                }
            }
            if (noInactiveAmmo)
            {
                GameObject powerup = Instantiate(_powerups[3], posToSpawn, Quaternion.identity, _powerupParent.transform);
                _ammoPowerupPool.Add(powerup);
            }
        }

        private void SpawnMissilePowerup()
        {
            if (_missileRareCount > 1)
            {
                bool noInactiveMissile = true;
                Vector3 posToSpawn = new Vector3(Random.Range(-(Helper.GetXPositionBounds()), Helper.GetXPositionBounds()), Helper.GetYUpperScreenBounds() + 2.5f, 0);

                foreach (GameObject itemInPool in _missilePowerupPool)
                {
                    if (itemInPool.activeSelf == false)
                    {
                        itemInPool.SetActive(true);
                        itemInPool.transform.position = posToSpawn;
                        noInactiveMissile = false;
                        break;
                    }
                }
                if (noInactiveMissile)
                {
                    GameObject powerup =
                    Instantiate(_powerups[5], posToSpawn, Quaternion.identity, _powerupParent.transform);
                    _missilePowerupPool.Add(powerup);
                }
                _missileRareCount = 0;
            }
            else
            {
                _missileRareCount++;
            }
        }
    }
}

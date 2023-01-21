using GameCanvas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace SpawnManager
{
    public class SpawnManager : MonoBehaviour
    {
        private int _enemySpawnCount = 0;
        private int _extraLifeRareCount = 0;
        private int _healthRareCount = 0;
        private int _lastPowerup = -1;
        private const int _MAX_ENEMIES_PER_WAVE = 5;
        private int _missileRareCount = 0;        
        private int _randomEnemy;
        private int _secondLastPowerup = -1;
        private float _spawnWait;
        private bool _canSpawn = false;
        private List<GameObject> _speedPowerupsPool = new List<GameObject>();
        private List<GameObject> _tripleShotPowerupsPool = new List<GameObject>();
        private List<GameObject> _shieldPowerupPool = new List<GameObject>();
        private List<GameObject> _enemyRandomPool = new List<GameObject>();
        private List<GameObject> _enemyAlienPool = new List<GameObject>();
        private List<GameObject> _enemyRedFighterPool = new List<GameObject>();
        private List<GameObject> _lifeStealerPowerupPool = new List<GameObject>();
        private List<GameObject> _enemyRegularPool = new List<GameObject>();
        private List<GameObject> _ammoPowerupPool = new List<GameObject>();
        private List<GameObject> _healthPowerupPool = new List<GameObject>();
        private List<GameObject> _missilePowerupPool = new List<GameObject>();
        private GameObject _enemyParent;
        private GameObject _powerupParent;
        private GameObject _asteroid;
        private GameCanvasManager _gameCanvas;
        private Shake _shaker;
        [Range(0, 100)][SerializeField] private int _waveCount = 0;
        [SerializeField] private GameObject[] _enemies;
        [SerializeField] private GameObject[] _powerups;

        void Start()
        {
            if (_enemies == null)
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

            _shaker = GameObject.Find("Main Camera").GetComponent<Shake>();
            if (_shaker == null)
            {
                Debug.LogError("Shake not found on SpawnManager");
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                _canSpawn = true;
                StartCoroutine(SpawnPowerups());
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                _canSpawn = false;
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                SpawnBossEnemy();
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
            bool canSpawnBoss = true;
            while (canSpawnEnemies)
            {
                yield return new WaitForSeconds(_spawnWait);

                switch (_waveCount)
                {
                    case 1:
                        _randomEnemy = 0;
                        break;
                    case 2 or 3:
                        _randomEnemy = Random.Range(0, 2);
                        break;
                    case 4:
                        _randomEnemy = Random.Range(0, 3);
                        break;
                    case 5:
                        _randomEnemy = Random.Range(0, 4);
                        break;
                    default:
                        _randomEnemy = Random.Range(0, _enemies.Length);
                        break;
                }

                if (_randomEnemy == 0)
                {
                    SpawnRegularEnemy();
                    _enemySpawnCount++;
                }
                else if (_randomEnemy == 1)
                {
                    SpawnRandomEnemy();
                    _enemySpawnCount++;
                }
                else if (_randomEnemy == 2)
                {
                    SpawnAlienEnemy();
                    _enemySpawnCount += 2;
                }
                else if (_waveCount % 5 == 0 && _randomEnemy == 3 && canSpawnBoss)
                {
                    _enemySpawnCount += 5;
                    canSpawnBoss = SpawnBossEnemy();
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
                GameObject newEnemy = Instantiate(_enemies[0], _enemyParent.transform);
                _enemyRegularPool.Add(newEnemy);
            }
            _spawnWait = Random.Range(1f, 5f);
        }

        private void SpawnRandomEnemy()
        {
            bool isActiveEnemy = true;

            foreach (GameObject itemInPool in _enemyRandomPool)
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
                GameObject newEnemy = Instantiate(_enemies[1], _enemyParent.transform);
                _enemyRandomPool.Add(newEnemy);
            }
            _spawnWait = Random.Range(1f, 5f);
        }

        private void SpawnAlienEnemy()
        {
            bool isActiveEnemy = true;

            foreach (GameObject itemInPool in _enemyAlienPool)
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
                GameObject newEnemy = Instantiate(_enemies[2], _enemyParent.transform);
                _enemyAlienPool.Add(newEnemy);
            }
            _spawnWait = Random.Range(1f, 5f);
        }

        private bool SpawnBossEnemy()
        {
            bool isActiveEnemy = true;

            foreach (GameObject itemInPool in _enemyRedFighterPool)
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
                GameObject newEnemy = Instantiate(_enemies[3], _enemyParent.transform);
                _enemyRedFighterPool.Add(newEnemy);
            }
            _shaker.EnableShake(3f, 3f, 3f);
            _spawnWait = Random.Range(1f, 5f);
            return false;
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
                    case 6:
                        SpawnLifeStealerPowerup();
                        break;
                    case 7:
                        SpawnExtraLifePowerup();
                        break;
                }
                yield return new WaitForSeconds(Random.Range(5f, 10f));
            }
        }

        private void SpawnHealthPowerup()
        {
            if (_healthRareCount > 1)
            {
                bool noInactivePowerup = true;
                Vector3 posToSpawn = new Vector3(Random.Range(-(Helper.GetXPositionBounds()), Helper.GetXPositionBounds()), Helper.GetYUpperScreenBounds() + 2.5f, 0);

                foreach (GameObject itemInPool in _healthPowerupPool)
                {
                    if (itemInPool.activeSelf == false)
                    {
                        itemInPool.SetActive(true);
                        itemInPool.transform.position = posToSpawn;
                        noInactivePowerup = false;
                        break;
                    }
                }
                if (noInactivePowerup)
                {
                    GameObject powerup = Instantiate(_powerups[_lastPowerup], posToSpawn, Quaternion.identity, _powerupParent.transform);
                    _healthPowerupPool.Add(powerup);
                }
                _healthRareCount = 0;
            }
            else
            {
                _healthRareCount++;
            }

        }

        private void SpawnTripleShotPowerup()
        {
            bool noInactivePowerup = true;
            Vector3 posToSpawn = new Vector3(Random.Range(-(Helper.GetXPositionBounds()), Helper.GetXPositionBounds()), Helper.GetYUpperScreenBounds() + 2.5f, 0);

            foreach (GameObject itemInPool in _tripleShotPowerupsPool)
            {
                if (itemInPool.activeSelf == false)
                {
                    itemInPool.SetActive(true);
                    itemInPool.transform.position = posToSpawn;
                    noInactivePowerup = false;
                    break;
                }
            }
            if (noInactivePowerup)
            {
                GameObject powerup =
                Instantiate(_powerups[_lastPowerup], posToSpawn, Quaternion.identity, _powerupParent.transform);
                _tripleShotPowerupsPool.Add(powerup);
            }
        }

        private void SpawnSpeedPowerup()
        {
            bool noInactivePowerup = true;
            Vector3 posToSpawn = new Vector3(Random.Range(-(Helper.GetXPositionBounds()), Helper.GetXPositionBounds()), Helper.GetYUpperScreenBounds() + 2.5f, 0);

            foreach (GameObject itemInPool in _speedPowerupsPool)
            {
                if (itemInPool.activeSelf == false)
                {
                    itemInPool.SetActive(true);
                    itemInPool.transform.position = posToSpawn;
                    noInactivePowerup = false;
                    break;
                }
            }
            if (noInactivePowerup)
            {
                GameObject powerup = Instantiate(_powerups[_lastPowerup], posToSpawn, Quaternion.identity, _powerupParent.transform);
                _speedPowerupsPool.Add(powerup);
            }
        }

        private void SpawnShieldPowerup()
        {
            bool noInactivePowerup = true;
            Vector3 posToSpawn = new Vector3(Random.Range(-(Helper.GetXPositionBounds()), Helper.GetXPositionBounds()), Helper.GetYUpperScreenBounds() + 2.5f, 0);

            foreach (GameObject itemInPool in _shieldPowerupPool)
            {
                if (itemInPool.activeSelf == false)
                {
                    itemInPool.SetActive(true);
                    itemInPool.transform.position = posToSpawn;
                    noInactivePowerup = false;
                    break;
                }
            }
            if (noInactivePowerup)
            {
                GameObject powerup = Instantiate(_powerups[_lastPowerup], posToSpawn, Quaternion.identity, _powerupParent.transform);
                _shieldPowerupPool.Add(powerup);
            }
        }

        private void SpawnAmmoPowerup()
        {
            bool noInactivePowerup = true;
            Vector3 posToSpawn = new Vector3(Random.Range(-(Helper.GetXPositionBounds()), Helper.GetXPositionBounds()), Helper.GetYUpperScreenBounds() + 2.5f, 0);

            foreach (GameObject itemInPool in _ammoPowerupPool)
            {
                if (itemInPool.activeSelf == false)
                {
                    itemInPool.SetActive(true);
                    itemInPool.transform.position = posToSpawn;
                    noInactivePowerup = false;
                    break;
                }
            }
            if (noInactivePowerup)
            {
                GameObject powerup = Instantiate(_powerups[_lastPowerup], posToSpawn, Quaternion.identity, _powerupParent.transform);
                _ammoPowerupPool.Add(powerup);
            }
        }

        private void SpawnMissilePowerup()
        {
            if (_missileRareCount > 1)
            {
                bool noInactivePowerup = true;
                Vector3 posToSpawn = new Vector3(Random.Range(-(Helper.GetXPositionBounds()), Helper.GetXPositionBounds()), Helper.GetYUpperScreenBounds() + 2.5f, 0);

                foreach (GameObject itemInPool in _missilePowerupPool)
                {
                    if (itemInPool.activeSelf == false)
                    {
                        itemInPool.SetActive(true);
                        itemInPool.transform.position = posToSpawn;
                        noInactivePowerup = false;
                        break;
                    }
                }
                if (noInactivePowerup)
                {
                    GameObject powerup =
                    Instantiate(_powerups[_lastPowerup], posToSpawn, Quaternion.identity, _powerupParent.transform);
                    _missilePowerupPool.Add(powerup);
                }
                _missileRareCount = 0;
            }
            else
            {
                _missileRareCount++;
            }
        }

        private void SpawnLifeStealerPowerup()
        {
            bool noInactivePowerup = true;
            Vector3 posToSpawn = new Vector3(Random.Range(-(Helper.GetXPositionBounds()), Helper.GetXPositionBounds()), Helper.GetYUpperScreenBounds() + 2.5f, 0);

            foreach (GameObject itemInPool in _lifeStealerPowerupPool)
            {
                if (itemInPool.activeSelf == false)
                {
                    itemInPool.SetActive(true);
                    itemInPool.transform.position = posToSpawn;
                    noInactivePowerup = false;
                    break;
                }
            }
            if (noInactivePowerup)
            {
                GameObject powerup =
                Instantiate(_powerups[_lastPowerup], posToSpawn, Quaternion.identity, _powerupParent.transform);
                _lifeStealerPowerupPool.Add(powerup);
            }
        }
        private void SpawnExtraLifePowerup()
        {
            if (_extraLifeRareCount > 1)
            {
                bool noInactivePowerup = true;
                Vector3 posToSpawn = new Vector3(Random.Range(-(Helper.GetXPositionBounds()), Helper.GetXPositionBounds()), Helper.GetYUpperScreenBounds() + 2.5f, 0);

                foreach (GameObject itemInPool in _lifeStealerPowerupPool)
                {
                    if (itemInPool.activeSelf == false)
                    {
                        itemInPool.SetActive(true);
                        itemInPool.transform.position = posToSpawn;
                        noInactivePowerup = false;
                        break;
                    }
                }
                if (noInactivePowerup)
                {
                    GameObject powerup =
                    Instantiate(_powerups[_lastPowerup], posToSpawn, Quaternion.identity, _powerupParent.transform);
                    _lifeStealerPowerupPool.Add(powerup);
                }
                _extraLifeRareCount = 0;
            }
            else
            {
                _extraLifeRareCount++;
            }
        }
    }
}

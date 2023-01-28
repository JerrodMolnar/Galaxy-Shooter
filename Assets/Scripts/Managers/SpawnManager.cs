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
        private int _lastPowerup = -1;
        private int _randomPowerup = -1;
        private const int _MAX_ENEMIES_PER_WAVE = 5;
        private int _lastEnemy = -1;
        private int _randomEnemy;
        private int _totalEnemyWeight = 0;
        private int _totalPowerupWeight = 0;
        private float _spawnWait;
        private bool _canSpawn = false;
        private List<GameObject> _speedPowerupsPool = new List<GameObject>();
        private List<GameObject> _tripleShotPowerupsPool = new List<GameObject>();
        private List<GameObject> _shieldPowerupPool = new List<GameObject>();
        private List<GameObject> _enemyRandomPool = new List<GameObject>();
        private List<GameObject> _enemyAlienPool = new List<GameObject>();
        private List<GameObject> _enemyDroidPool = new List<GameObject>();
        private List<GameObject> _enemyRedFighterPool = new List<GameObject>();
        private List<GameObject> _enemyTieFighterPool = new List<GameObject>();
        private List<GameObject> _lifeStealerPowerupPool = new List<GameObject>();
        private List<GameObject> _enemyRegularPool = new List<GameObject>();
        private List<GameObject> _ammoPowerupPool = new List<GameObject>();
        private List<GameObject> _healthPowerupPool = new List<GameObject>();
        private List<GameObject> _missilePowerupPool = new List<GameObject>();
        private List<GameObject> _homingMissilePowerupPool = new List<GameObject>();
        private GameObject _enemyParent;
        private GameObject _powerupParent;
        private GameObject _asteroid;
        private GameCanvasManager _gameCanvas;
        private Shake _shaker;
        [Range(0, 100)][SerializeField] private int _waveCount = 0;
        [SerializeField] private GameObject[] _enemies;
        [SerializeField] private GameObject[] _powerups;
        [Range(0, 1000)][SerializeField] private int[] _enemyWeights;
        [Range(0, 1000)][SerializeField] private int[] _powerupWeights;

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

            foreach (int i in _enemyWeights)
            {
                _totalEnemyWeight += i;
            }

            foreach (int i in _powerupWeights)
            {
                _totalPowerupWeight += i;
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
                _randomPowerup = 6;
                SpawnHomingMissile();
                _randomEnemy = 0;
                SpawnRegularEnemy();
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

        public void StopSpawn()
        {
            _canSpawn = false;
            StopCoroutine(SpawnEnemies());
            StopCoroutine(SpawnPowerups());
        }

        private int RandomEnemy()
        {
            int randomEnemy = Random.Range(0, _totalEnemyWeight);
            int enemyChosen = -1;

            for (int i = 0; i < _enemyWeights.Length; i++)
            {
                if (randomEnemy <= _enemyWeights[i])
                {
                    enemyChosen = i;
                    break;
                }
                else
                {
                    randomEnemy -= _enemyWeights[i];
                }
            }
            return enemyChosen;
        }

        private IEnumerator SpawnEnemies()
        {
            yield return new WaitForSeconds(5f);
            bool canSpawnEnemies = _canSpawn;
            bool canSpawnBoss = true;
            while (canSpawnEnemies)
            {
                yield return new WaitForSeconds(_spawnWait);
                if (_waveCount == 1)
                {
                    _randomEnemy = 0;
                    _lastEnemy = -1;
                }
                else if (_waveCount == 2)
                {
                    if (_lastEnemy == 1)
                    {
                        _randomEnemy = 0;
                    }
                    else
                    {
                        _randomEnemy = 1;
                    }
                }
                else
                {
                    _randomEnemy = RandomEnemy();

                    while (_lastEnemy == _randomEnemy)
                    {
                        _randomEnemy = RandomEnemy();
                        yield return new WaitForSeconds(0.05f);
                    }
                }

                _lastEnemy = _randomEnemy;

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
                else if (_randomEnemy == 2 && _waveCount >= 3)
                {
                    SpawnTieFighter();
                    _enemySpawnCount += 2;
                }
                else if (_randomEnemy == 3 && _waveCount >= 4)
                {
                    SpawnAlienEnemy();
                    _enemySpawnCount += 3;
                }
                else if (_randomEnemy == 4 && _waveCount >= 4)
                {
                    SpawnDroidEnemy();
                    _enemySpawnCount += 2;
                }
                else if (_waveCount >= 5 && _waveCount % 5 == 0 && _randomEnemy == 4 && canSpawnBoss)
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
                        if (GameObject.Find("Game Manager").TryGetComponent(out GameManager gm))
                        {
                            if (!gm.IsGameOver())
                            {
                                _gameCanvas.EndWave(_waveCount);
                            }
                        }
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
                GameObject newEnemy = Instantiate(_enemies[_randomEnemy], _enemyParent.transform);
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
                GameObject newEnemy = Instantiate(_enemies[_randomEnemy], _enemyParent.transform);
                _enemyRandomPool.Add(newEnemy);
            }
            _spawnWait = Random.Range(1f, 5f);
        }

        private void SpawnTieFighter()
        {
            bool isActiveEnemy = true;

            foreach (GameObject itemInPool in _enemyTieFighterPool)
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
                GameObject newEnemy = Instantiate(_enemies[_randomEnemy], _enemyParent.transform);
                _enemyTieFighterPool.Add(newEnemy);
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
                GameObject newEnemy = Instantiate(_enemies[_randomEnemy], _enemyParent.transform);
                _enemyAlienPool.Add(newEnemy);
            }
            _spawnWait = Random.Range(1f, 5f);
        }

        private void SpawnDroidEnemy()
        {
            bool isActiveEnemy = true;

            foreach (GameObject itemInPool in _enemyDroidPool)
            {
                if (itemInPool.activeInHierarchy == false)
                {
                    itemInPool.transform.GetChild(0).GetComponent<Enemy>().enabled = true;
                    itemInPool.SetActive(true);
                    itemInPool.transform.GetChild(0).GetComponent<PolygonCollider2D>().enabled = true;
                    isActiveEnemy = false;
                    break;
                }
            }
            if (isActiveEnemy)
            {
                GameObject newEnemy = Instantiate(_enemies[_randomEnemy], _enemyParent.transform);
                _enemyDroidPool.Add(newEnemy);
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
                GameObject newEnemy = Instantiate(_enemies[_randomEnemy], _enemyParent.transform);
                _enemyRedFighterPool.Add(newEnemy);
            }
            _shaker.EnableShake(2f, 2f, 1.5f);
            _spawnWait = Random.Range(1f, 5f);
            return false;
        }

        private int RandomPowerup()
        {
            int randomPowerup = Random.Range(0, _totalPowerupWeight);
            int powerupChosen = -1;

            for (int i = 0; i < _powerupWeights.Length; i++)
            {
                if (randomPowerup <= _powerupWeights[i])
                {
                    powerupChosen = i;
                    break;
                }
                else
                {
                    randomPowerup -= _powerupWeights[i];
                }
            }
            return powerupChosen;
        }

        private IEnumerator SpawnPowerups()
        {
            yield return new WaitForSeconds(5f);
            while (_canSpawn)
            {
                _randomPowerup = RandomPowerup();
                while (_randomPowerup == _lastPowerup)
                {
                    _randomPowerup = RandomPowerup();
                    yield return new WaitForSeconds(0.1f);
                }
                _lastPowerup = _randomPowerup;

                switch (_lastPowerup)
                {
                    case 0:
                        SpawnAmmoPowerup();
                        break;
                    case 1:
                        SpawnSpeedPowerup();
                        break;
                    case 2:
                        SpawnTripleShotPowerup();
                        break;
                    case 3:
                        SpawnLifeStealerPowerup();
                        break;
                    case 4:
                        SpawnShieldPowerup();
                        break;
                    case 5:
                        SpawnHealthPowerup();
                        break;
                    case 6:
                        SpawnHomingMissile();
                        break;
                    case 7:
                        SpawnMissilePowerup();
                        break;
                    case 8:
                        SpawnExtraLifePowerup();
                        break;
                    default:
                        Debug.Log("No powerup spawned.");
                        break;
                }
                yield return new WaitForSeconds(Random.Range(5f, 10f));
            }
        }

        private void SpawnHealthPowerup()
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
                GameObject powerup = Instantiate(_powerups[_randomPowerup], posToSpawn, Quaternion.identity, _powerupParent.transform);
                _healthPowerupPool.Add(powerup);
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
                Instantiate(_powerups[_randomPowerup], posToSpawn, Quaternion.identity, _powerupParent.transform);
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
                GameObject powerup = Instantiate(_powerups[_randomPowerup], posToSpawn, Quaternion.identity, _powerupParent.transform);
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
                GameObject powerup = Instantiate(_powerups[_randomPowerup], posToSpawn, Quaternion.identity, _powerupParent.transform);
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
                GameObject powerup = Instantiate(_powerups[_randomPowerup], posToSpawn, Quaternion.identity, _powerupParent.transform);
                _ammoPowerupPool.Add(powerup);
            }
        }

        private void SpawnMissilePowerup()
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
                Instantiate(_powerups[_randomPowerup], posToSpawn, Quaternion.identity, _powerupParent.transform);
                _missilePowerupPool.Add(powerup);
            }
        }

        private void SpawnHomingMissile()
        {
            bool noInactivePowerup = true;
            Vector3 posToSpawn = new Vector3(Random.Range(-(Helper.GetXPositionBounds()), Helper.GetXPositionBounds()), Helper.GetYUpperScreenBounds() + 2.5f, 0);

            foreach (GameObject itemInPool in _homingMissilePowerupPool)
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
                GameObject powerup = Instantiate(_powerups[_randomPowerup], posToSpawn, Quaternion.identity, _powerupParent.transform);
                _homingMissilePowerupPool.Add(powerup);
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
                Instantiate(_powerups[_randomPowerup], posToSpawn, Quaternion.identity, _powerupParent.transform);
                _lifeStealerPowerupPool.Add(powerup);
            }
        }

        private void SpawnExtraLifePowerup()
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
                Instantiate(_powerups[_randomPowerup], posToSpawn, Quaternion.identity, _powerupParent.transform);
                _lifeStealerPowerupPool.Add(powerup);
            }
        }
    }
}

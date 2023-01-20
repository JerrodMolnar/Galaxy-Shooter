using GameCanvas;
using ProjectileFire;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Utility;
using Color = UnityEngine.Color;

namespace Health
{
    public class Health : MonoBehaviour
    {
        private static int _score = 0;
        private int _health;
        private SpawnManager.SpawnManager _spawnManager;
        private const float _REAPPEAR_WAIT_TIME = 2f;
        [SerializeField] private int _hitsOnShield = 0;
        private GameObject _shieldsVisualizer;
        private GameCanvasManager _gameCanvasManager;
        private Animator _animator;
        private float _nextHit = -1f;
        private const float _HITWAIT = 1f;
        private GameObject _rightEngineFire;
        private GameObject _leftEngineFire;
        private int _fireNumber = 0;
        private AudioSource _audioSource;
        [SerializeField] private AudioClip _hurtClip;
        private Shake _shaker;
        [SerializeField] private int _maxHealth = 100;
        [SerializeField] private int _lives = 3;
        [SerializeField] private int _maxLives = 5;
        [SerializeField] private AudioClip _explosionClip;
        [SerializeField] private GameObject _shieldPrefab;

        private void Start()
        {
            _gameCanvasManager = GameObject.Find("Canvas").GetComponent<GameCanvasManager>();
            if (_gameCanvasManager == null)
            {
                Debug.LogError("Game Canvas Manager not found on Health Script on " + name);
            }

            _animator = gameObject.GetComponent<Animator>();
            if (_animator == null)
            {
                Debug.LogError("Animator not found on Health Script on " + name);
            }

            if (tag == "Player")
            {
                _gameCanvasManager.UpdateHealth(_health);
                _gameCanvasManager.UpdateLives(_lives);
                _gameCanvasManager.UpdateScore(_score);
                _shieldsVisualizer = gameObject.transform.GetChild(0).gameObject;
                if (_shieldsVisualizer == null)
                {
                    Debug.LogError("Shield visualizer not found on Health script on " + tag.ToString());
                }

                _leftEngineFire = transform.GetChild(2).gameObject;
                _rightEngineFire = transform.GetChild(3).gameObject;
                if (_rightEngineFire == null)
                {
                    Debug.LogError("Right engine fire is not found on Health script on " + name);
                }
                if (_leftEngineFire == null)
                {
                    Debug.LogError("Left engine fire is not found on Health script on " + name);
                }
            }

            _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager.SpawnManager>();
            if (_spawnManager == null)
            {
                Debug.LogError("Spawn Manager not found on Health script " + name);
            }

            _audioSource = gameObject.GetComponent<AudioSource>();
            if (_audioSource == null)
            {
                _audioSource = this.AddComponent<AudioSource>();
            }
            _audioSource.playOnAwake = false;

            _shaker = GameObject.Find("Main Camera").GetComponent<Shake>();
            if (_shaker == null)
            {
                Debug.LogError("Shake not found on Health script on " + name);
            }

            if (_shieldPrefab == null)
            {
                Debug.LogError("Shield prefab not fount on Health script on " + name);
            }

            if (_hurtClip == null)
            {
                Debug.LogError("Hurt clip not found on Health script on " + name);
            }
        }

        private void OnEnable()
        {
            _health = _maxHealth;
            if (_shieldsVisualizer == null)
            {
                int childCount = transform.childCount;
                for (int i = 0; i < childCount; i++)
                {
                    if (transform.GetChild(i).name == "Shield")
                    {
                        _shieldsVisualizer = transform.GetChild(i).gameObject;
                        break;
                    }
                }
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                if (CompareTag("Enemy"))
                {
                    TakeLife();
                }
            }
        }

        public void DamageTaken(int damageAmount, bool isFromPlayer)
        {
            if (_nextHit < Time.time)
            {
                _nextHit = Time.time + _HITWAIT;
                if (tag == "Player")
                {
                    _audioSource.clip = _hurtClip;
                    _audioSource.volume = 0.075f;
                    _audioSource.Play();
                    _shaker.EnableShake();

                    if (_hitsOnShield > 0)
                    {
                        HitShield();
                    }
                    else
                    {
                        StartCoroutine(ColorFlasher(0.5f));
                        _health -= damageAmount;

                        if (_health < 0)
                        {
                            _health = 0;
                        }
                        EngineDamage(false);
                        _gameCanvasManager.UpdateHealth(_health);
                    }
                }
                else
                {
                    StartCoroutine(ColorFlasher(_HITWAIT / 8));
                    if (_hitsOnShield > 0)
                    {
                        HitShield();
                    }
                    else
                    {
                        _audioSource.clip = _hurtClip;
                        _audioSource.volume = 0.075f;
                        _audioSource.Play();
                        _health -= damageAmount;
                        if (isFromPlayer)
                        {
                            _score += damageAmount;
                            _gameCanvasManager.UpdateScore(_score);
                        }
                    }
                }

                if (_health <= 0)
                {
                    TakeLife();
                }
            }
        }

        private void EngineDamage(bool isHealed)
        {
            float healthPercent = (float)_health / (float)_maxHealth;
            if (!isHealed)
            {
                if (healthPercent < 0.66f && _fireNumber == 0)
                {
                    _fireNumber = 1;
                    if (_rightEngineFire.activeSelf)
                    {
                        _leftEngineFire.gameObject.SetActive(true);
                    }
                    else if (_leftEngineFire.activeSelf)
                    {
                        _rightEngineFire.gameObject.SetActive(true);
                    }
                    else
                    {
                        int randomFire = Random.Range(0, 2);
                        switch (randomFire)
                        {
                            case 0:
                                _leftEngineFire.gameObject.SetActive(true);
                                break;
                            case 1:
                                _rightEngineFire.gameObject.SetActive(true);
                                break;
                        }
                    }
                }
                else if (healthPercent < 0.33 && _fireNumber == 1)
                {
                    _fireNumber = 2;
                    if (_rightEngineFire.activeSelf)
                    {
                        _leftEngineFire.gameObject.SetActive(true);
                    }
                    else if (_leftEngineFire.activeSelf)
                    {
                        _rightEngineFire.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                if (healthPercent > 0.66 && _fireNumber == 1)
                {
                    _fireNumber = 0;
                    _leftEngineFire.gameObject.SetActive(false);
                    _rightEngineFire.gameObject.SetActive(false);
                }
                else if (healthPercent > 0.33 && healthPercent < 0.66 && _fireNumber == 2)
                {
                    _fireNumber = 1;
                    int randomFire = Random.Range(0, 2);
                    switch (randomFire)
                    {
                        case 0:
                            _leftEngineFire.gameObject.SetActive(true);
                            _rightEngineFire.gameObject.SetActive(false);
                            break;
                        case 1:
                            _rightEngineFire.gameObject.SetActive(true);
                            _leftEngineFire.gameObject.SetActive(false);
                            break;
                    }
                }
            }
        }

        public void ExtraLife()
        {
            if (_lives < _maxLives)
            {
                _lives++;
                if (tag == "Player")
                {
                    _gameCanvasManager.UpdateLives(_lives);
                }
            }
        }

        public void TakeLife()
        {
            _lives -= 1;

            if (tag == "Player")
            {
                GetComponent<Player>().enabled = false;
                _gameCanvasManager.UpdateHealth(_health);
                transform.GetChild(1).gameObject.SetActive(false);
                _leftEngineFire.SetActive(false);
                _rightEngineFire.SetActive(false);
                _animator.SetTrigger("IsDead");
            }
            else
            {
                GetComponent<Enemy>().enabled = false;
            }

            _audioSource.clip = _explosionClip;
            _audioSource.volume = 0.25f;
            _audioSource.Play();
            GetComponent<PolygonCollider2D>().enabled = false;

            if (_lives > 0)
            {
                StartCoroutine(Reappear());
            }
            else
            {
                if (tag == "Player")
                {
                    _gameCanvasManager.UpdateHealth(_health);
                    _gameCanvasManager.UpdateLives(_lives);
                    _gameCanvasManager.UpdateGameOver(true);
                    _spawnManager.StartSpawn(false);
                    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    foreach (GameObject enemy in enemies)
                    {
                        if (enemy.activeInHierarchy == true)
                        {
                            enemy.GetComponent<Enemy>().enabled = false;
                            enemy.GetComponent<Animator>().SetTrigger("IsDead");
                            _audioSource.Play();
                        }
                    }
                    GameObject[] powerups = GameObject.FindGameObjectsWithTag("Powerup");
                    foreach (GameObject powerup in powerups)
                    {
                        if (powerup.activeInHierarchy == true)
                        {
                            powerup.SetActive(false);
                        }
                    }
                }
                else
                {
                    if (name == "Alien Enemy(Clone)")
                    {
                        transform.GetChild(0).gameObject.SetActive(false);
                    }
                    _animator.SetTrigger("IsDead");
                }
            }
        }

        public void EnableShield(int hitsOnShield)
        {
            if (_shieldsVisualizer == null)
            {
                _shieldsVisualizer = Instantiate(_shieldPrefab, transform.position, Quaternion.identity, transform);
            }
            _shieldsVisualizer.SetActive(false);
            _hitsOnShield = hitsOnShield;
            HitShield();
            _shieldsVisualizer.SetActive(true);
        }

        private void HitShield()
        {
            _hitsOnShield--;
            switch (_hitsOnShield)
            {
                case 3:
                    _shieldsVisualizer.GetComponent<SpriteRenderer>().color = Color.white;
                    break;
                case 2:
                    _shieldsVisualizer.GetComponent<SpriteRenderer>().color = Color.green;
                    break;
                case 1:
                    _shieldsVisualizer.GetComponent<SpriteRenderer>().color = Color.red;
                    break;
                default:
                    _shieldsVisualizer.gameObject.SetActive(false);
                    break;
            }
        }

        public void DamageHealed(int healAmount)
        {
            if (_health + healAmount > _maxHealth)
            {
                _health = _maxHealth;
            }
            else
            {
                _health += healAmount;
            }

            if (tag == "Player")
            {
                _gameCanvasManager.UpdateHealth(_health);
                EngineDamage(true);
            }
        }

        public static void SetScoreTo0()
        {
            _score = 0;
        }

        public IEnumerator Reappear()
        {

            yield return new WaitForSeconds(_REAPPEAR_WAIT_TIME);
            if (tag == "Player")
            {
                transform.position = new Vector3(0, -2.5f, 0);
                _animator.SetTrigger("IsReappear");
                transform.GetChild(1).gameObject.SetActive(true); //Thrusters
                GetComponent<PolygonCollider2D>().enabled = true;
                GetComponent<Player>().enabled = true;
                _health = _maxHealth;
                _gameCanvasManager.UpdateHealth(_health);
                _gameCanvasManager.UpdateLives(_lives);
                _hitsOnShield = 2;
                HitShield();
                yield return new WaitForSeconds(1f);
                HitShield();
                _fireNumber = 0;
                _leftEngineFire.SetActive(false);
                _rightEngineFire.SetActive(false);
                GetComponent<FireProjectiles>().AmmoPickup();
            }
            else
            {
                transform.position = new Vector3(Random.Range(-(Helper.GetXPositionBounds()), Helper.GetXPositionBounds()), Helper.GetYUpperScreenBounds(), 0);
                GetComponent<SpriteRenderer>().enabled = true;
                GetComponent<PolygonCollider2D>().enabled = true;
                GetComponent<Enemy>().enabled = true;
            }
        }

        private IEnumerator ColorFlasher(float flashTime)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            for (int i = 0; i < 4; i++)
            {
                spriteRenderer.color = Color.red;
                yield return new WaitForSeconds(flashTime);
                spriteRenderer.color = Color.white;
                yield return new WaitForSeconds(flashTime);
            }

        }
    }
}

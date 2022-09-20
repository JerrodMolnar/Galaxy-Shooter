using System.Collections;
using UnityEngine;
using Utility;
using GameCanvas;
using Color = UnityEngine.Color;
using System.Reflection.Emit;
using System.Drawing;

namespace Health
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int _maxHealth = 100;
        [SerializeField] private int _lives = 3;
        [SerializeField] private const int _maxLives = 5;
        private static int _score = 0;
        private int _health;
        private SpawnManager.SpawnManager _spawnManager;
        private float _reappearWaitTime = 2f;
        private int _hitsOnShield = 0;
        private int _maxShieldHits = 3;
        private GameObject _shieldsVisualizer;
        private GameCanvasManager _gameCanvasManager;
        private Animator _animator;
        private float _nextHit = -1f;
        private const float _HITWAIT = 1f;
        private GameObject _rightEngineFire;
        private GameObject _leftEngineFire;
        private int _fireNumber = 0;

        private void Start()
        {
            _health = _maxHealth;
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
                Debug.LogError("Spawn Manager not found on Health script");
            }

        }

        private void ShieldEnabled()
        {
            Color color;
            switch (_hitsOnShield)
            {
                case 3:
                    color = new Color(255, 255, 0);
                    _shieldsVisualizer.GetComponent<SpriteRenderer>().color = color;
                    _hitsOnShield--;
                    break;
                case 2:
                    color = new Color(255, 0, 0);
                    _shieldsVisualizer.GetComponent<SpriteRenderer>().color = color;
                    _hitsOnShield--;
                    break;
                case 1:
                    _hitsOnShield--;
                    color = new Color(255, 255, 255);
                    _shieldsVisualizer.GetComponent<SpriteRenderer>().color = color;
                    _shieldsVisualizer.SetActive(false);
                    break;
            }
        }

        public void DamageTaken(int damageAmount)
        {
            if (_nextHit < Time.time)
            {
                _nextHit = Time.time + _HITWAIT;

                if (tag == "Player")
                {

                    if (_hitsOnShield > 0)
                    {
                        ShieldEnabled();
                    }
                    else
                    {
                        _health -= damageAmount;

                        if (_health < 0)
                        {
                            _health = 0;
                        }
                        float healthPercent = (float) _health / (float) _maxHealth;
                        Debug.Log(healthPercent + "%");
                        if (healthPercent < 0.66f && _fireNumber == 0)
                        {
                            _fireNumber++;
                            EngineDamage();
                        }
                        else if (healthPercent < 0.33 && _fireNumber == 1)
                        {
                            _fireNumber++;
                            EngineDamage();
                        }
                        _gameCanvasManager.UpdateHealth(_health);
                    }
                }
                else
                {
                    _health -= damageAmount;
                    _score += damageAmount;
                    _gameCanvasManager.UpdateScore(_score);
                }

                if (_health <= 0)
                {
                    TakeLife();
                }
            }
        }

        private void EngineDamage()
        {
            if (_rightEngineFire.activeSelf)
            {
                _leftEngineFire.gameObject.SetActive(true);
            }
            else if (_leftEngineFire.activeSelf)
            {
                _rightEngineFire.SetActive(true);
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

        public static void SetScoreTo0()
        {
            _score = 0;
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
            }
        }

        public void EnableShield()
        {
            _hitsOnShield = _maxShieldHits;
            _shieldsVisualizer.SetActive(true);
            _shieldsVisualizer.GetComponent<SpriteRenderer>().color = Color.white;
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
            }
            else
            {
                GetComponent<Enemy>().enabled = false;
            }
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
                    _spawnManager.Spawn(false);
                    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                    foreach (GameObject enemy in enemies)
                        enemy.GetComponent<Animator>().SetTrigger("IsDead");
                    GameObject[] powerups = GameObject.FindGameObjectsWithTag("Powerup");
                    foreach (GameObject powerup in powerups)
                    {
                        powerup.SetActive(false);
                    }
                    gameObject.SetActive(false);
                }
                else
                {
                    _animator.SetTrigger("IsDead");
                }
            }
        }

        public IEnumerator Reappear()
        {

            GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(_reappearWaitTime);
            if (tag == "Player")
            {
                transform.position = new Vector3(0, -2.5f, 0);
                GetComponent<SpriteRenderer>().enabled = true;
                transform.GetChild(1).gameObject.SetActive(true);
                GetComponent<PolygonCollider2D>().enabled = true;
                GetComponent<Player>().enabled = true;
                _health = _maxHealth;
                _gameCanvasManager.UpdateHealth(_health);
                _gameCanvasManager.UpdateLives(_lives);
                _hitsOnShield = 2;
                ShieldEnabled();
                yield return new WaitForSeconds(1f);
                ShieldEnabled();
                _fireNumber = 0;
                _leftEngineFire.SetActive(false);
                _rightEngineFire.SetActive(false);
            }
            else
            {
                transform.position = new Vector3(Random.Range(-(Helper.GetXPositionBounds()), Helper.GetXPositionBounds()), Helper.GetYUpperScreenBounds(), 0);
                GetComponent<SpriteRenderer>().enabled = true;
                GetComponent<PolygonCollider2D>().enabled = true;
                GetComponent<Enemy>().enabled = true;
            }
        }
    }
}

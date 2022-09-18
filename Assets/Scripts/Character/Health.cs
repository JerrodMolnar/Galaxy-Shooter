using System.Collections;
using UnityEngine;
using Utility;
using GameCanvas;

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
            }

            _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager.SpawnManager>();

            if (_spawnManager == null)
            {
                Debug.LogError("Spawn Manager not found on Health script");
            }
        }

        public void DamageTaken(int damageAmount)
        {
            if (tag == "Player")
            {

                if (_hitsOnShield > 0)
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
                else
                {
                    _health -= damageAmount;
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
        }

        public void TakeLife()
        {
            _lives -= 1;

            if (tag == "Player")
            {
                GetComponent<Player>().enabled = false;
                _gameCanvasManager.UpdateHealth(_health);
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
                GetComponent<PolygonCollider2D>().enabled = true;
                GetComponent<Player>().enabled = true;
                _health = _maxHealth;
                _gameCanvasManager.UpdateHealth(_health);
                _gameCanvasManager.UpdateLives(_lives);
                _hitsOnShield = 1;
                yield return new WaitForSeconds(1f);
                _hitsOnShield = 0;
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

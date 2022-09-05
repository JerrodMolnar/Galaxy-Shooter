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
        private int _health;
        private SpawnManager.SpawnManager _spawnManager;
        private float _reappearWaitTime = 2f;
        private bool _isInvincible = false;

        private void Start()
        {
            _health = _maxHealth;
            if (tag == "Player")
            {
                GameCanvasManager.health = _health;
                GameCanvasManager.lives = _lives;
            }

            _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager.SpawnManager>();

            if (_spawnManager == null)
            {
                Debug.LogError("Spawn Manager not found on Health script");
            }
        }

        public void DamageTaken(int damageAmount)
        {
            if (!_isInvincible)
            {
                _health -= damageAmount;
                if (tag == "Enemy")
                {
                    GameCanvasManager.score += damageAmount;
                }
            }

            if (_health <= 0)
            {
                TakeLife();
            }
            else if (tag == "Player")
            {
                GameCanvasManager.health = _health;
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
                GameCanvasManager.health = _health;
            }
        }

        public void TakeLife()
        {
            _lives -= 1;
            _health = 0;

            if (tag == "Player")
            {
                GetComponent<Player>().enabled = false;
                GameCanvasManager.health = _health;
            }
            else
            {
                GetComponent<Enemy>().enabled = false;
            }
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<PolygonCollider2D>().enabled = false;
            StartCoroutine(Reappear());
        }

        public IEnumerator Reappear()
        {
            float invincibleTime = 3f;
            yield return new WaitForSeconds(_reappearWaitTime);
            if (tag == "Player" && _lives > 0)
            {
                transform.position = new Vector3(0, -2.5f, 0);
                GetComponent<SpriteRenderer>().enabled = true;
                GetComponent<PolygonCollider2D>().enabled = true;
                GetComponent<Player>().enabled = true;
                _health = _maxHealth;
                GameCanvasManager.health = _health;
                GameCanvasManager.lives = _lives;
                StartCoroutine(Invincible(invincibleTime));
            }
            else if (_lives > 0)
            {
                transform.position = new Vector3(Random.Range(-(Helper.GetXPositionBounds()), Helper.GetXPositionBounds()), Helper.GetYUpperScreenBounds(), 0);
                GetComponent<SpriteRenderer>().enabled = true;
                GetComponent<PolygonCollider2D>().enabled = true;
                GetComponent<Enemy>().enabled = true;
                StartCoroutine(Invincible(invincibleTime));
            }
            else if (tag == "Player")
            {
                GameCanvasManager.health = _health;
                GameCanvasManager.lives = _lives;
                _spawnManager.Spawn(false);
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject enemy in enemies)
                    enemy.SetActive(false);
                gameObject.SetActive(false);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private IEnumerator Invincible(float timeInvincible)
        {
            _isInvincible = true;
            yield return new WaitForSeconds(timeInvincible);
            _isInvincible = false;
        }
    }
}

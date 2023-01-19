using UnityEngine;
using Utility;

namespace ProjectileType
{
    public class Missile : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 5.0f;
        private bool _isPlayerMissile = false;
        private float _missileTime = -1f;
        private float _missileCoolDown = 10f;
        private GameObject _enemyFound;
        private GameObject _player;
        Rigidbody2D rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            _player = GameObject.FindGameObjectWithTag("Player");
            if (_player != null )
            {
                Debug.LogError("Player not found on Missile Script on " + name);
            }
        }

        private void Update()
        {
            MissileMovement();
        }

        public void SetShooter(bool isPlayerMissile)
        {
            _isPlayerMissile = isPlayerMissile;
            _missileTime = _missileCoolDown + Time.time;
            _enemyFound = CheckForEnemy();
        }

        private void MissileMovement()
        {            
            if (_isPlayerMissile)
            {
                if (_missileTime > Time.time)
                {
                    if (_enemyFound != null)
                    {
                        if (_enemyFound.GetComponent<Enemy>().isActiveAndEnabled)
                        {
                            Vector2 direction = _enemyFound.transform.position - transform.position;
                            direction.Normalize();
                            float rotateAmount = Vector3.Cross(direction, transform.up).z;
                            rb.angularVelocity = -rotateAmount * 200f;
                            rb.velocity = transform.up * _moveSpeed;
                        }
                        else
                        {
                            CheckForEnemy();
                        }
                    }
                    else if (_enemyFound == null)
                    {
                        transform.rotation = Quaternion.identity;
                        rb.velocity = transform.up * _moveSpeed;
                    }
                }
                else
                {
                    transform.rotation = Quaternion.identity;
                    rb.velocity = transform.up * _moveSpeed;
                }

                if (transform.position.y > Helper.GetYUpperScreenBounds() + 10f)
                {
                    gameObject.SetActive(false);
                    transform.position = Vector3.zero;
                }
            }
            else
            {
                Vector2 direction = _player.transform.position - transform.position;
                direction.Normalize();
                float rotateAmount = Vector3.Cross(direction, transform.up).z;
                rb.angularVelocity = -rotateAmount * 200f;
                rb.velocity = transform.up * _moveSpeed;
            }
        }

        private GameObject CheckForEnemy()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            float leastDistance = Mathf.Infinity;
            int indexLeast = -1;
            for (int i = 0; i < enemies.Length; i ++)
            {
                if (enemies[i].GetComponent<Enemy>().isActiveAndEnabled)
                {
                    float distance = Vector3.Distance(enemies[i].transform.position, transform.position);
                    if (distance < leastDistance)
                    {
                        leastDistance = distance;
                        indexLeast = i;
                    }
                }
            }
            if (indexLeast != -1)
            {
                return enemies[indexLeast];
            }
            return null;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.activeSelf)
            {
                if (other.CompareTag("Enemy") && _isPlayerMissile)
                {
                    other.GetComponent<Health.Health>()?.DamageTaken(25, true);
                    transform.rotation = Quaternion.identity;
                    _enemyFound = CheckForEnemy();
                }
                else if (other.CompareTag("Player") && !_isPlayerMissile)
                {
                    other.GetComponent<Health.Health>()?.DamageTaken(25, false);
                    gameObject.SetActive(false);
                }
                else if (other.CompareTag("Enemy"))
                {
                    other.GetComponent<Health.Health>()?.DamageTaken(25, false);
                }
            }
        }
    }
}

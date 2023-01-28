using UnityEngine;
using Utility;

namespace ProjectileType
{
    public class HomingMissile : MonoBehaviour
    {
        [Range(0, 10f)][SerializeField] private float _moveSpeed = 5.0f;
        private bool _isPlayerHomingMissile = false;
        private GameObject _enemyFound;
        private GameObject _player;

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            if (_player == null)
            {
                Debug.LogError("Player not found on Missile Script on " + name);
            }
        }

        private void Update()
        {
            MissileMovement();
        }

        public void SetShooter(bool isPlayerHomingMissile)
        {
            _isPlayerHomingMissile = isPlayerHomingMissile;
            if (_isPlayerHomingMissile )
            {
                transform.eulerAngles = Vector3.forward * 90;
            }
            else
            {
                transform.eulerAngles = Vector3.forward * -90;
            }
            _enemyFound = CheckForEnemy();
        }

        private void MissileMovement()
        {
            if (_isPlayerHomingMissile)
            {
                if (_enemyFound != null && _enemyFound.GetComponent<Enemy>().isActiveAndEnabled)
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, GetAngle(_enemyFound.transform.position),
                        200f * Time.deltaTime);
                    transform.position = Vector2.MoveTowards(transform.position, _enemyFound.transform.position, _moveSpeed * Time.deltaTime);
                }
                else if (_enemyFound == null)
                {
                    transform.Translate(Vector3.right * _moveSpeed * Time.deltaTime);
                }

                if (transform.position.y > Helper.GetYUpperScreenBounds() + 10f)
                {
                    gameObject.SetActive(false);
                    transform.position = Vector3.zero;
                }
            }
            else
            {
                if (_player.gameObject.activeInHierarchy)
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, GetAngle(_player.transform.position),
                        200f * Time.deltaTime);
                    transform.position = Vector2.MoveTowards(transform.position, _player.transform.position,
                        _moveSpeed * Time.deltaTime);
                }
                else
                {
                    transform.Translate(Vector3.left * _moveSpeed * Time.deltaTime);
                }
            }
        }

        private Quaternion GetAngle(Vector3 targetPosition)
        {
            float angle = Mathf.Atan2(targetPosition.y - transform.position.y, targetPosition.x - transform.position.x)
                * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            return rotation;
        }

        private GameObject CheckForEnemy()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            float leastDistance = Mathf.Infinity;
            int indexLeast = -1;
            for (int i = 0; i < enemies.Length; i++)
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
                if (other.CompareTag("Enemy") && _isPlayerHomingMissile)
                {
                    other.GetComponent<Health.Health>()?.DamageTaken(25, true);
                    gameObject.SetActive(false);
                }
                else if (other.CompareTag("Player") && !_isPlayerHomingMissile)
                {
                    other.GetComponent<Health.Health>()?.DamageTaken(25, false);
                    gameObject.SetActive(false);
                }
                else if (other.CompareTag("Enemy"))
                {
                    other.GetComponent<Health.Health>()?.DamageTaken(25, false);
                    gameObject.SetActive(false);
                }
            }
        }
    }
}

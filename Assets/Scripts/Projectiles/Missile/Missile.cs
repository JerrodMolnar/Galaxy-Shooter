using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace ProjectileType
{
    public class Missile : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 5.0f;
        private bool _isPlayerMissile = false;
        private float _missileTime = -1f;
        private float _missileCoolDown = 5f;
        private GameObject _enemyFound;

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
                if (_enemyFound != null)
                {
                    Vector3 direction = _enemyFound.transform.position - transform.position;
                    transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
                    direction = direction.normalized;
                    transform.Translate(direction * _moveSpeed * Time.deltaTime);
                }
                else if (_enemyFound == null || _missileTime < Time.time)
                {
                    transform.rotation = Quaternion.identity;
                    transform.Translate(Vector3.up * _moveSpeed * Time.deltaTime);
                }

                if (transform.position.y > Helper.GetYUpperScreenBounds() + 10f)
                {
                    gameObject.SetActive(false);
                    transform.position = Vector3.zero;
                }                
            }           
        }

        private GameObject CheckForEnemy()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject enemy in enemies)
            {
                if (enemy.GetComponent<Enemy>().isActiveAndEnabled)
                {
                    return enemy;
                }
            }
            return null;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.activeSelf)
            {
                if (other.CompareTag("Enemy") && _isPlayerMissile)
                {
                    other.GetComponent<Health.Health>()?.DamageTaken(5);
                    transform.rotation = Quaternion.identity;
                    _enemyFound = CheckForEnemy();
                }
            }
            if (other.tag == "Laser")
            {
                other.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }
}

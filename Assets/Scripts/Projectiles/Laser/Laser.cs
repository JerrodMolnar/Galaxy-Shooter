using UnityEngine;
using Utility;

namespace ProjectileType
{

    public class Laser : MonoBehaviour
    {
        private bool _isPlayerLaser = false;
        [Range(0, 100)] [SerializeField] private int _damageAmount = 5;
        [Range(0, 25f)] [SerializeField] private float _moveSpeed = 8.0f;

        private void Update()
        {
            LaserMovement();
        }

        public void SetShooter(bool isPlayerLaser)
        {
            _isPlayerLaser = isPlayerLaser;
        }

        private void LaserMovement()
        {
            if (_isPlayerLaser)
            {
                if (transform.position.y > Helper.GetYUpperScreenBounds() + 2.5f)
                {
                    gameObject.SetActive(false);
                    transform.position = Vector3.zero;
                }
                else
                {
                    transform.Translate(Vector3.up * _moveSpeed * Time.deltaTime);
                }
            }
            else
            {
                if (transform.position.y < Helper.GetYLowerBounds() - 2.5f)
                {
                    transform.position = Vector3.zero;
                    gameObject.SetActive(false);
                }
                else
                {
                    transform.Translate(Vector3.down * _moveSpeed * Time.deltaTime);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.activeSelf)
            {
                if (collision.CompareTag("Enemy") && _isPlayerLaser)
                {                    
                    collision.GetComponent<Health.Health>()?.DamageTaken(_damageAmount, true);
                }
                else if (collision.CompareTag("Player"))
                {
                    collision.GetComponent<Health.Health>()?.DamageTaken(_damageAmount * 2, false);
                }
                else if (collision.CompareTag("Enemy"))
                {
                    collision.GetComponent<Health.Health>()?.DamageTaken(_damageAmount, false);
                }
                gameObject.SetActive(false);
            }
            if (collision.tag == "Projectile")
            {
                if (collision.gameObject.TryGetComponent<Mine>(out Mine mine))
                {
                    mine.BlowUpSequence();
                }
                else
                {
                    collision.gameObject.SetActive(false);
                }
                gameObject.SetActive(false);
            }
        }
    }
}

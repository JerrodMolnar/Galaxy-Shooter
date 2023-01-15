using UnityEngine;
using Utility;

namespace ProjectileType
{

    public class Laser : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 8.0f;
        private bool _isPlayerLaser = false;
        [SerializeField] private int _damageAmount = 5;

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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.activeSelf)
            {
                if (other.CompareTag("Enemy"))
                {                    
                    other.GetComponent<Health.Health>()?.DamageTaken(_damageAmount);
                    gameObject.SetActive(false);
                }
                else if (other.CompareTag("Player"))
                {
                    other.GetComponent<Health.Health>()?.DamageTaken(_damageAmount * 2);
                    gameObject.SetActive(false);
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

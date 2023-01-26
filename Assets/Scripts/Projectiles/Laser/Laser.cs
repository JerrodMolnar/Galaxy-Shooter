using UnityEngine;
using Utility;

namespace ProjectileType
{

    public class Laser : MonoBehaviour
    {
        private bool _isBehindPlayer = false;
        private bool _isPlayerLaser = false;
        [Range(0, 100)][SerializeField] private int _damageAmount = 5;
        [Range(0, 25f)][SerializeField] private float _moveSpeed = 8.0f;

        private void Update()
        {
            LaserMovement();
        }

        public void SetShooter(bool isPlayerLaser, bool isBehindPlayer, bool isFromTieFighter, Transform playerTransform)
        {
            _isPlayerLaser = isPlayerLaser;
            _isBehindPlayer = isBehindPlayer;
            if (isFromTieFighter && playerTransform != null)
            {
                float xDifference = playerTransform.position.x - transform.position.x;

                if (xDifference > 10)
                {
                    if (isBehindPlayer)
                    {
                        transform.eulerAngles = new Vector3(0, 0, -75);
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, 0, 75);
                    }
                }
                else if (xDifference < -10)
                {
                    if (isBehindPlayer)
                    {
                        transform.eulerAngles = new Vector3(0, 0, 75);
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, 0, -75);
                    }
                }
                if (xDifference > 6)
                {
                    if (isBehindPlayer)
                    {
                        transform.eulerAngles = new Vector3(0, 0, -40);
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, 0, 40);
                    }
                }
                else if (xDifference < -6)
                {
                    if (isBehindPlayer)
                    {
                        transform.eulerAngles = new Vector3(0, 0, 40);
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, 0, -40);
                    }
                }
                else if (xDifference > 3)
                {
                    if (isBehindPlayer)
                    {
                        transform.eulerAngles = new Vector3(0, 0, -20);
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, 0, 20);
                    }
                }
                else if (xDifference < -3)
                {
                    if (isBehindPlayer)
                    {
                        transform.eulerAngles = new Vector3(0, 0, 20);
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, 0, -20);
                    }
                }
                else
                {
                    transform.eulerAngles = Vector3.zero;
                }

            }
            else
            {
                transform.eulerAngles = Vector3.zero;
            }
        }

        private void LaserMovement()
        {
            if (_isBehindPlayer || _isPlayerLaser)
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

            if (transform.position.x > Helper.GetXPositionBounds() || transform.position.x < -Helper.GetXPositionBounds())
            {
                gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.activeSelf)
            {
                if (collision.CompareTag("Enemy") && _isPlayerLaser)
                {
                    collision.GetComponent<Health.Health>()?.DamageTaken(_damageAmount, true);
                    gameObject.SetActive(false);
                }
                else if (collision.CompareTag("Player"))
                {
                    collision.GetComponent<Health.Health>()?.DamageTaken(_damageAmount * 2, false);
                    gameObject.SetActive(false);
                }
                else if (collision.CompareTag("Enemy"))
                {
                    collision.GetComponent<Health.Health>()?.DamageTaken(_damageAmount, false);
                    gameObject.SetActive(false);
                }
            }
            if (collision.tag == "Projectile")
            {
                if (collision.gameObject.TryGetComponent<Mine>(out Mine mine))
                {
                    mine.BlowUpSequence();
                }
                else if (collision.gameObject.name == "Missile(Clone)")
                {
                    return;
                }
                else
                {
                    collision.gameObject.SetActive(false);
                    gameObject.SetActive(false);
                }
            }
        }
    }
}

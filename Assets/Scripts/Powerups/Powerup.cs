using ProjectileFire;
using UnityEngine;
using Utility;

namespace Powerup
{
    public class Powerup : MonoBehaviour
    {
        private float _moveSpeed = 1.5f;
        private GameObject _player;
        [SerializeField] private _powerupID _currentID;
        [SerializeField] private AudioClip _powerupClip;

        private enum _powerupID
        {
            TripleShot,
            Speed,
            Shield,
            Ammo,
            Health,
            HomingMissile,
            MissileShot,
            LifeSteal,
            ExtraLife
        }

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.C))
            {
                if (_player != null)
                {
                    Vector2 direction = _player.transform.position - transform.position;
                    direction.Normalize();
                    transform.Translate(direction * _moveSpeed * 5 * Time.deltaTime);
                }
            }
            else
            {
                Movement();
            }
        }

        private void Movement()
        {
            transform.Translate(Vector3.down * _moveSpeed * Time.deltaTime);
            if (transform.position.y < Helper.GetYLowerBounds() - 3f)
            {
                transform.position = new Vector3(transform.position.x, Helper.GetYUpperScreenBounds() + 3f, 0);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
            {
                switch (_currentID)
                {
                    case _powerupID.TripleShot:
                        collision.GetComponent<FireProjectiles>().TripleShotEnable();
                        gameObject.SetActive(false);
                        break;
                    case _powerupID.Speed:
                        if (collision.CompareTag("Player"))
                        {
                            collision.GetComponent<Player>().SpeedPowerup();
                        }
                        else
                        {
                            collision.GetComponent<Enemy>().SpeedPowerup();
                        }
                        gameObject.SetActive(false);
                        break;
                    case _powerupID.Shield:
                        int hitsOnShield;
                        if (collision.CompareTag("Player"))
                        {
                            hitsOnShield = 3;
                        }
                        else
                        {
                            hitsOnShield = 1;
                        }
                        collision.GetComponent<Health.Health>().EnableShield(hitsOnShield);
                        gameObject.SetActive(false);
                        break;
                    case _powerupID.Ammo:
                        if (collision.CompareTag("Player"))
                        {
                            collision.GetComponent<FireProjectiles>().AmmoPickup();
                            gameObject.SetActive(false);
                        }
                        break;
                    case _powerupID.Health:
                        collision.GetComponent<Health.Health>().DamageHealed(33);
                        gameObject.SetActive(false);
                        break;
                    case _powerupID.HomingMissile:
                        collision.GetComponent<FireProjectiles>();
                        break;
                    case _powerupID.MissileShot:
                        collision.GetComponent<FireProjectiles>().MissileEnable();
                        gameObject.SetActive(false);
                        break;
                    case _powerupID.LifeSteal:
                        if ((collision.CompareTag("Enemy") && collision.GetComponent<Enemy>().GetEnemyType() != 5) || 
                            collision.CompareTag("Player"))
                        {
                            collision.GetComponent<Health.Health>().TakeLife();
                            gameObject.SetActive(false);
                        }
                        break;
                    case _powerupID.ExtraLife:
                        collision.GetComponent<Health.Health>().ExtraLife();
                        gameObject.SetActive(false);
                        break;
                    default:
                        Debug.LogError("No Powerup ID match found.");
                        break;
                }
                AudioSource.PlayClipAtPoint(_powerupClip, transform.position);
            }
            else if (collision.CompareTag("Projectile"))
            {
                collision.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }
}

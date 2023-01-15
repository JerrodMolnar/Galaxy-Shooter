using ProjectileFire;
using UnityEngine;
using Utility;

namespace Powerup
{
    public class Powerup : MonoBehaviour
    {
        private float _speed = 1.5f;
        [SerializeField] private _powerupID _currentID;
        [SerializeField] private AudioClip _powerupClip;
        private enum _powerupID
        {
            TripleShot,
            Speed,
            Shield,
            Ammo,
            Health,
            MissileShot,
            LifeSteal
        }


        void Update()
        {
            Movement();
        }

        private void Movement()
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
            if (transform.position.y < Helper.GetYLowerBounds() - 3f)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {                
                switch (_currentID)
                {
                    case _powerupID.TripleShot:
                        collision.GetComponent<FireProjectiles>().TripleShotEnable();
                        gameObject.SetActive(false);
                        break;
                    case _powerupID.Speed:
                        collision.GetComponent<Player>().SpeedPowerup();
                        gameObject.SetActive(false);
                        break;
                    case _powerupID.Shield:
                        collision.GetComponent<Health.Health>().EnableShield();
                        gameObject.SetActive(false);
                        break;
                    case _powerupID.Ammo:
                        collision.GetComponent<FireProjectiles>().AmmoPickup();
                        gameObject.SetActive(false);
                        break;
                    case _powerupID.Health:
                        collision.GetComponent<Health.Health>().DamageHealed(33);
                        gameObject.SetActive(false);
                        break;
                    case _powerupID.MissileShot:
                        collision.GetComponent<FireProjectiles>().MissileEnable();
                        gameObject.SetActive(false);
                        break;
                    case _powerupID.LifeSteal:
                        collision.GetComponent<Health.Health>().TakeLife();
                        gameObject.SetActive(false);
                        break;
                    default:
                        Debug.LogError("No Powerup ID match found.");
                        break;
                }
                AudioSource.PlayClipAtPoint(_powerupClip, transform.position);
            }
        }
    }
}

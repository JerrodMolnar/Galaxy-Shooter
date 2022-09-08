using UnityEngine;
using Utility;

namespace Powerup
{
    public class Powerup : MonoBehaviour
    {
        [SerializeField] private float _speed = 3.0f;
        [SerializeField] private _powerupID _currentID;
        private enum _powerupID
        {
            TripleShot,
            Speed,
            Shield
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
                        collision.GetComponent<ProjectileFire.FireProjectiles>().TripleShotEnable();
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
                    default:
                        Debug.LogError("No Powerup ID match found.");
                        break;
                }
            }
        }
    }
}

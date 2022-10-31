using Unity.VisualScripting;
using UnityEngine;
using Utility;

namespace Powerup
{
    public class Powerup : MonoBehaviour
    {
        [SerializeField] private float _speed = 3.0f;
        [SerializeField] private _powerupID _currentID;
        private AudioSource _audioSource;
        [SerializeField] private AudioClip _powerupClip;
        private enum _powerupID
        {
            TripleShot,
            Speed,
            Shield
        }

        private void Start()
        {

            _audioSource = transform.AddComponent<AudioSource>();
            if (_audioSource == null)
            {
                Debug.LogError("AudioSource not found on Powerup Script on " + name);
            }
            else
            {
                _audioSource.playOnAwake = false;
                _audioSource.volume = 1;
                _audioSource.clip = _powerupClip;
                _audioSource.priority = 100;
            }
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
                _audioSource.Play();
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

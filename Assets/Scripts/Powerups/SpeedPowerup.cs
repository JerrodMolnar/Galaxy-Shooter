using UnityEngine;
using Utility;

namespace Powerup
{    public class SpeedPowerup : MonoBehaviour
    {
        [SerializeField] float _speed = 4f;

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
                collision.GetComponent<Player>().SpeedPowerup();
                gameObject.SetActive(false);
            }
        }
    }
}

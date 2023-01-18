using UnityEngine;
using Utility;

namespace ProjectileType
{
    public class TripleShot : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 8.0f;
        private bool _isPlayerShot;

        void Update()
        {
            TripleShotMovement();
        }

        private void TripleShotMovement()
        {
            if (_isPlayerShot)
            {
                if (transform.position.y > Helper.GetYUpperScreenBounds() + 5f)
                {
                    gameObject.GetComponentInChildren<Transform>().gameObject.SetActive(false);
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).gameObject.SetActive(false);
                    }
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
                if (transform.position.y < Helper.GetYUpperScreenBounds() - 5f)
                {
                    gameObject.GetComponentInChildren<Transform>().gameObject.SetActive(false);
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).gameObject.SetActive(false);
                    }
                    gameObject.SetActive(false);
                    transform.position = Vector3.zero;
                }
                else
                {
                    transform.Translate(Vector3.down * _moveSpeed * Time.deltaTime);
                }
            }
        }

        public void SetPlayerShot(bool isPlayerShot)
        {
            _isPlayerShot = isPlayerShot;
        }
    }
}



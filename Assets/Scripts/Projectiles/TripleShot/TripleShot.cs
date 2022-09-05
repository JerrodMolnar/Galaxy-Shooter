using UnityEngine;
using Utility;

namespace ProjectileType
{
    public class TripleShot : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 8.0f;

        void Update()
        {
            TripleShotMovement();
        }

        private void TripleShotMovement()
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
    }
}



using UnityEngine;
using Utility;

namespace ProjectileType
{
    public class TripleShot : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 8.0f;
        private bool _isPlayerShot = false;

        void Update()
        {
            TripleShotMovement();
        }

        private void TripleShotMovement()
        {
            transform.Translate(_moveSpeed * Time.deltaTime * Vector3.up);

            if (transform.position.y > Helper.GetYUpperScreenBounds() + 5f || transform.position.y < Helper.GetYLowerBounds() - 5f)
            {
                gameObject.GetComponentInChildren<Transform>().gameObject.SetActive(false);
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
                gameObject.SetActive(false);
                transform.position = Vector3.zero;
            }
        }

        public void SetPlayerShot(bool isPlayerShot)
        {
            _isPlayerShot = isPlayerShot;
        }

        public bool IsPlayerShot()
        {
            return _isPlayerShot;
        }
    }
}



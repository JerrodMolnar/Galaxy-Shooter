using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectilePool
{
    public class TripleShotPool : MonoBehaviour
    {
        [SerializeField] private GameObject _tripleShotPrefab;
        private List<GameObject> _tripleShotList = new List<GameObject>();

        private void Start()
        {
            if (_tripleShotPrefab == null)
            {
                Debug.LogError("Triple shot prefab missing on " + this.name);
            }
        }

        public void ShootTripleShot(bool isShotByPlayer, Vector3 shotPosition)
        {
            bool isNoActiveTripleShot = true;
            foreach (GameObject tripleShot in _tripleShotList)
            {
                if (tripleShot.activeSelf == false)
                {
                    tripleShot.SetActive(true);
                    for (int i = 0; i < tripleShot.transform.childCount; i++)
                    {
                        tripleShot.transform.GetChild(i).gameObject.SetActive(true);
                    }
                    tripleShot.transform.position = shotPosition;
                    isNoActiveTripleShot = false;
                    break;
                }
            }
            if (isNoActiveTripleShot)
            {
                GameObject newTripleShot = Instantiate(_tripleShotPrefab, shotPosition, Quaternion.identity, transform);
                _tripleShotList.Add(newTripleShot);
            }
        }
    }
}

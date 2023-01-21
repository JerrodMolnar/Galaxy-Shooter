using ProjectileType;
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
            Vector3 playerAngle = new Vector3(0, 0, 0);
            Vector3 enemyAngle = new Vector3(0, 0, 180);

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
                    if (!isShotByPlayer)
                    {
                        tripleShot.transform.eulerAngles = enemyAngle;
                    }
                    else
                    {
                        tripleShot.transform.eulerAngles = playerAngle;
                    }
                    tripleShot.GetComponent<TripleShot>().SetPlayerShot(isShotByPlayer);
                    isNoActiveTripleShot = false;
                    break;
                }
            }
            if (isNoActiveTripleShot)
            {
                GameObject tripleShot;
                if (!isShotByPlayer)
                {
                    tripleShot = Instantiate(_tripleShotPrefab, shotPosition, Quaternion.Euler(enemyAngle), transform);
                }
                else
                {
                    tripleShot = Instantiate(_tripleShotPrefab, shotPosition, Quaternion.Euler(playerAngle), transform);
                }
                tripleShot.GetComponent<TripleShot>().SetPlayerShot(isShotByPlayer);
                _tripleShotList.Add(tripleShot);
            }
        }
    }
}

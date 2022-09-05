using System.Collections.Generic;
using UnityEngine;

namespace ProjectilePool
{
    public class LaserPool : MonoBehaviour
    {
        [SerializeField] private GameObject _laserPrefab;
        private List<GameObject> _laserList = new List<GameObject>();

        void Start()
        {
            if (_laserPrefab == null)
            {
                Debug.LogError("Laser prefab missing on " + name);
            }
        }

        public void ShootLaserFromPool(bool isPlayerLaser, Vector3 shotPosition)
        {
            if (_laserPrefab != null)
            {
                bool isNoActiveLaser = true;

                foreach (GameObject itemInPool in _laserList)
                {
                    if (itemInPool.activeSelf == false)
                    {
                        itemInPool.SetActive(true);
                        itemInPool.GetComponent<ProjectileType.Laser>().SetShooter(isPlayerLaser);
                        itemInPool.transform.position = shotPosition;
                        isNoActiveLaser = false;
                        break;
                    }
                }
                if (isNoActiveLaser)
                {
                    GameObject newProjectile = Instantiate(_laserPrefab, shotPosition, Quaternion.identity, transform);
                    newProjectile.GetComponent<ProjectileType.Laser>().SetShooter(isPlayerLaser);
                    _laserList.Add(newProjectile);
                }
            }
        }
    }
}


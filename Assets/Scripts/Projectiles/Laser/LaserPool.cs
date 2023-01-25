using System.Collections.Generic;
using UnityEngine;

namespace ProjectilePool
{
    public class LaserPool : MonoBehaviour
    {
        [SerializeField] private GameObject _laserPrefab;
        private List<GameObject> _laserList = new List<GameObject>();
        private GameObject _laserSelected;

        void Start()
        {
            if (_laserPrefab == null)
            {
                Debug.LogError("Laser prefab missing on " + name);
            }
        }

        public void ShootLaserFromPool(bool isPlayerLaser, Vector3 shotPosition)
        {
            ShootLaserFromPool(isPlayerLaser, shotPosition, false, false);
        }

        public void ShootLaserFromPool(bool isPlayerLaser, Vector3 shotPosition, bool isBehindPlayer, bool isFromTieFighter)
        {
            if (_laserPrefab != null)
            {
                bool isNoActiveLaser = true;

                for (int i = 0; i < _laserList.Count; i++)
                {
                    if (_laserList[i].activeSelf == false)
                    {
                        _laserList[i].SetActive(true);
                        _laserList[i].transform.position = shotPosition;
                        _laserSelected = _laserList[i];
                        isNoActiveLaser = false;
                        break;
                    }
                }

                if (isNoActiveLaser)
                {
                    _laserSelected = Instantiate(_laserPrefab, shotPosition, Quaternion.identity, transform);
                    _laserList.Add(_laserSelected);
                }
                ProjectileType.Laser laser = _laserSelected.GetComponent<ProjectileType.Laser>();
                laser.SetShooter(isPlayerLaser, isBehindPlayer, isFromTieFighter);
            }
        }
    }
}


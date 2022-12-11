using System.Collections.Generic;
using UnityEngine;

namespace ProjectilePool
{
    public class MissilePool : MonoBehaviour
    {
        [SerializeField] private GameObject _missilePrefab;
        private List<GameObject> _missileList = new List<GameObject>();

        void Start()
        {
            if (_missilePrefab == null)
            {
                Debug.LogError("Missile prefab missing on " + name);
            }
        }

        public void ShootMissileFromPool(bool isPlayerMissile, Vector3 shotPosition)
        {
            if (_missilePrefab != null)
            {
                bool isNoActiveMissile = true;

                foreach (GameObject itemInPool in _missileList)
                {
                    if (itemInPool.activeSelf == false)
                    {
                        itemInPool.SetActive(true);
                        itemInPool.GetComponent<ProjectileType.Missile>().SetShooter(isPlayerMissile);
                        itemInPool.transform.position = shotPosition;
                        isNoActiveMissile = false;
                        break;
                    }
                }
                if (isNoActiveMissile)
                {
                    GameObject newProjectile = Instantiate(_missilePrefab, shotPosition, Quaternion.identity, transform);
                    newProjectile.GetComponent<ProjectileType.Missile>().SetShooter(isPlayerMissile);
                    _missileList.Add(newProjectile);
                }
            }
        }
    }
}

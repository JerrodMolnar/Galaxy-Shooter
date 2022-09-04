using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ProjectileType;

namespace Projectile
{

    public class FireProjectiles : MonoBehaviour
    {
        private static GameObject _laserPoolParent;
        private static List<GameObject> _laserList = new List<GameObject>();
        [SerializeField] private GameObject _laser;
        private Vector3 _laserShootPosition;
        private bool _isPlayerShot = false;

        // Start is called before the first frame update
        void Start()
        {
            if (_laserPoolParent == null)
            {
                _laserPoolParent = new GameObject("Laser Object Pool");
            }

            if (_laser == null)
            {
                Debug.LogError("Laser Prefab not found");
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ShootProjectile(int projectileType)
        {
            switch (projectileType)
            {
                case 0: //laser
                    FireLaser();
                    break;
            }
        }

        private void FireLaser()
        {
            if (tag == "Player")
            {
                _isPlayerShot = true;
                _laserShootPosition = new Vector3(transform.position.x, transform.position.y + 1f, 0);
            }
            else
            {
                _isPlayerShot = false;
                _laserShootPosition = new Vector3(transform.position.x, transform.position.y - 1f, 0);
            }

            if (_laser != null)
            {
                bool isNoActiveLaser = true;

                foreach (GameObject itemInPool in _laserList)
                {
                    if (itemInPool.activeSelf == false)
                    {
                        itemInPool.SetActive(true);
                        itemInPool.transform.position = _laserShootPosition;
                        itemInPool.GetComponent<Laser>().SetShooter(_isPlayerShot);
                        isNoActiveLaser = false;
                        break;
                    }
                }
                if (isNoActiveLaser)
                {
                    GameObject newProjectile = Instantiate(_laser, _laserShootPosition, Quaternion.identity, _laserPoolParent.transform);
                    _laserList.Add(newProjectile);
                    newProjectile.GetComponent<Laser>().SetShooter(_isPlayerShot);
                }
            }
        }

    }
}

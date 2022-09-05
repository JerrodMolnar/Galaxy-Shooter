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
        private static GameObject _tripleShotParent;
        private static List<GameObject> _laserList = new List<GameObject>();
        private static List<GameObject> _tripleShotList = new List<GameObject>();
        [SerializeField] private GameObject _laserPrefab;
        [SerializeField] private GameObject _tripleShotPrefab;
        private Vector3 _laserShootPosition;
        private bool _isPlayerShot = false;
        [SerializeField] private bool _isTripleShotActive = false;

        // Start is called before the first frame update
        void Start()
        {
            if (_laserPoolParent == null)
            {
                _laserPoolParent = new GameObject("Laser Object Pool");
            }

            if (_tripleShotParent == null)
            {
                _tripleShotParent = new GameObject("Triple Shot Pool");
                _tripleShotParent.transform.SetParent(_laserPoolParent.transform);
            }

            if (_laserPrefab == null)
            {
                Debug.LogError("Laser Prefab not found");
            }
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
                
                if (_isTripleShotActive)
                {
                    _laserShootPosition = Vector3.zero;
                }
                else
                {
                    _laserShootPosition = new Vector3(transform.position.x, transform.position.y + 1.25f, 0);
                }
            }
            else
            {
                _isPlayerShot = false;
                _laserShootPosition = new Vector3(transform.position.x, transform.position.y - 1f, 0);
            }

            if (_laserPrefab != null && (!_isTripleShotActive || !_isPlayerShot))
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
                    GameObject newProjectile = Instantiate(_laserPrefab, _laserShootPosition, Quaternion.identity, _laserPoolParent.transform);
                    _laserList.Add(newProjectile);
                    newProjectile.GetComponent<Laser>().SetShooter(_isPlayerShot);
                }
            }
            else
            {
                Debug.Log("Else statement enter");
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
                        tripleShot.transform.position = _laserShootPosition;
                        Laser[] lasers = tripleShot.GetComponentsInChildren<Laser>();
                        foreach (Laser laser in lasers)
                        {
                            laser.SetShooter(true);
                        }
                        isNoActiveTripleShot = false;
                        break;
                    }
                }
                if (isNoActiveTripleShot)
                {
                    GameObject newTripleShot = Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity, _tripleShotParent.transform);
                    Laser[] lasers = newTripleShot.GetComponentsInChildren<Laser>();
                    foreach (Laser laser in lasers)
                    {
                        laser.SetShooter(true);
                    }
                    _tripleShotList.Add(newTripleShot);
                }
            }
        }

    }
}

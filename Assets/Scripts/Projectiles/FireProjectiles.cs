using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectilePool;

namespace ProjectileFire
{

    public class FireProjectiles : MonoBehaviour
    {
        private LaserPool _laserPooling;
        private TripleShotPool _tripleShotPool;
        private Vector3 _laserShootPosition;
        private bool _isPlayerShot = false;
        [SerializeField] private bool _isTripleShotActive = false;
        private float _tripleShotCoolDownWait = 10f;
        private float _tripleShotCoolDown = -1;

        // Start is called before the first frame update
        void Start()
        {
            _laserPooling = GameObject.Find("Laser Pool").GetComponent<LaserPool>();
            if (_laserPooling == null)
            {
                Debug.LogError("Laser Pool not found on FireProjectiles on " + name);
            }

            _tripleShotPool = GameObject.Find("Triple Shot Pool").GetComponent<TripleShotPool>();
            if (_tripleShotPool == null)
            {
                Debug.LogError("Triple shot pool not found on FireProjectiles on " + name);
            }
        }

        public void ShootProjectile(int projectileType)
        {
            switch (projectileType)
            {
                case 0: //laser
                    if (_isTripleShotActive && tag == "Player")
                    {
                        FireTripleShot();
                        if (_tripleShotCoolDown > Time.time)
                        {
                            _isTripleShotActive = false;
                        }
                    }
                    else
                    {
                        FireLaser();
                    }
                    break;
            }
        }

        public void TripleShotEnable()
        {
            _isTripleShotActive = true;
            _tripleShotCoolDown = Time.time + _tripleShotCoolDownWait;
        }

        private void FireTripleShot()
        {
            _tripleShotPool.ShootTripleShot(true, transform.position);
        }

        private void FireLaser()
        {
            if (tag == "Player")
            {
                _isPlayerShot = true;
                _laserShootPosition = new Vector3(transform.position.x, transform.position.y + 1.25f, 0);
            }
            else
            {
                _isPlayerShot = false;
                _laserShootPosition = new Vector3(transform.position.x, transform.position.y - 1f, 0);
            }
            _laserPooling.ShootLaserFromPool(_isPlayerShot, _laserShootPosition);
        }
    }
}

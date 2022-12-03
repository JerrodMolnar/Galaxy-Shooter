using UnityEngine;
using ProjectilePool;
using Unity.VisualScripting;

namespace ProjectileFire
{

    public class FireProjectiles : MonoBehaviour
    {
        private LaserPool _laserPooling;
        private TripleShotPool _tripleShotPool;
        private Vector3 _laserShootPosition;
        private bool _isPlayerShot = false;
        [SerializeField] private bool _isTripleShotActive = false;
        private float _tripleShotCoolDownWait = 5f;
        private float _tripleShotCoolDown = -1;
        private AudioSource _audioSource;
        [SerializeField] private AudioClip _laserSoundClip;
        [SerializeField] private AudioClip _outOfAmmoClip;
        private int _ammoCount = 0;
        private int _maxAmmoCount = 15;

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

            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null)
            {
                _audioSource = transform.AddComponent<AudioSource>();
            }
            _audioSource.playOnAwake = false;
            _audioSource.volume = 0.1f;
            _audioSource.priority = 20;

            _ammoCount = _maxAmmoCount;
        }

        public void ShootProjectile(int projectileType)
        {
            switch (projectileType)
            {
                case 0:
                    if (_isTripleShotActive && tag == "Player")
                    {
                        FireTripleShot();
                        if (_tripleShotCoolDown < Time.time)
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

        public void AmmoPickup()
        {
            _ammoCount = _maxAmmoCount;
        }

        private void FireTripleShot()
        {
            _tripleShotPool.ShootTripleShot(true, transform.position);
        }

        private void FireLaser()
        {
            if (tag == "Player")
            {
                if (_ammoCount > 0)
                {
                    _audioSource.clip = _laserSoundClip;
                    _audioSource.Play();
                    _isPlayerShot = true;
                    _laserShootPosition = new Vector3(transform.position.x, transform.position.y + 1.25f, 0);
                    _ammoCount--;
                    _laserPooling.ShootLaserFromPool(_isPlayerShot, _laserShootPosition);
                }
                else
                {
                    _audioSource.clip = _outOfAmmoClip;
                    _audioSource.Play();
                }                
            }
            else
            {
                _audioSource.clip = _laserSoundClip;
                _audioSource.Play();
                _isPlayerShot = false;
                _laserShootPosition = new Vector3(transform.position.x, transform.position.y - 1f, 0);
                _laserPooling.ShootLaserFromPool(_isPlayerShot, _laserShootPosition);
            }            
        }
    }
}

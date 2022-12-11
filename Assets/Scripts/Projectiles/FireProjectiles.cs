using UnityEngine;
using ProjectilePool;
using Unity.VisualScripting;

namespace ProjectileFire
{
    public class FireProjectiles : MonoBehaviour
    {
        private LaserPool _laserPool;
        private TripleShotPool _tripleShotPool;
        private MissilePool _missilePool;
        private Vector3 _laserShootPosition;
        private bool _isPlayerShot = false;
        [SerializeField] private bool _isTripleShotActive = false;
        [SerializeField] private bool _isMissileEnabled = false;
        private AudioSource _audioSource;
        [SerializeField] private AudioClip _laserSoundClip;
        [SerializeField] private AudioClip _outOfAmmoClip;
        [SerializeField] private AudioClip _missileClip;
        private int _ammoCount = 0;
        private int _maxAmmoCount = 15;
        private float _tripleShotCoolDownWait = 5f;
        private float _tripleShotCoolDown = -1;

        void Start()
        {
            _laserPool = GameObject.Find("Laser Pool").GetComponent<LaserPool>();
            if (_laserPool == null)
            {
                Debug.LogError("Laser Pool not found on FireProjectiles on " + name);
            }

            _tripleShotPool = GameObject.Find("Triple Shot Pool").GetComponent<TripleShotPool>();
            if (_tripleShotPool == null)
            {
                Debug.LogError("Triple shot pool not found on FireProjectiles on " + name);
            }

            _missilePool = GameObject.Find("Missile Pool").GetComponent<MissilePool>();
            if (_missilePool == null)
            {
                Debug.LogError("Missile pool not found on FireProjectiles on " + name);
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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                MissileEnable();
            }
        }

        public void ShootProjectile()
        {

            if (_isTripleShotActive && tag == "Player" && !_isMissileEnabled)
            {
                FireTripleShot();
                if (_tripleShotCoolDown < Time.time)
                {
                    _isTripleShotActive = false;
                }
            }
            else if (!_isMissileEnabled)
            {
                FireLaser();
            }
            else
            {
                FireMissile();
            }
        }

        public void TripleShotEnable()
        {
            _isTripleShotActive = true;
            _tripleShotCoolDown = Time.time + _tripleShotCoolDownWait;
        }

        public void MissileEnable()
        {
            _isMissileEnabled = true;
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
                    _laserPool.ShootLaserFromPool(_isPlayerShot, _laserShootPosition);
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
                _laserPool.ShootLaserFromPool(_isPlayerShot, _laserShootPosition);
            }
        }

        private void FireMissile()
        {
            if (tag == "Player")
            {
                _audioSource.clip = _missileClip;
                _audioSource.volume = 1f;
                _audioSource.Play();
                _audioSource.volume = 0.1f;
                _isPlayerShot = true;
                _laserShootPosition = new Vector3(transform.position.x, transform.position.y + 1.85f, 0);
                _missilePool.ShootMissileFromPool(_isPlayerShot, _laserShootPosition);
                _isMissileEnabled = false;
            }
        }
    }
}

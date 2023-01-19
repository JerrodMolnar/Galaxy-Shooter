using UnityEngine;
using ProjectilePool;
using Unity.VisualScripting;
using GameCanvas;
using System.Collections;
using System.Collections.Generic;

namespace ProjectileFire
{
    public class FireProjectiles : MonoBehaviour
    {
        private LaserPool _laserPool;
        private TripleShotPool _tripleShotPool;
        private MissilePool _missilePool;
        private Vector3 _laserShootPosition;
        private bool _isPlayerShot = false;
        private AudioSource _audioSource;
        private static int _ammoCount = 0;
        private static int _maxAmmoCount = 15;
        private int _enemyType;
        private float _tripleShotCoolDownWait = 5f;
        private float _tripleShotCoolDown = -1;
        private bool _tractorBeamActive = false;
        private GameCanvasManager _gameCanvas;
        private GameObject _tractorBeam;
        [SerializeField] private bool _isTripleShotActive = false;
        [SerializeField] private bool _isMissileEnabled = false;
        [SerializeField] private AudioClip _laserSoundClip;
        [SerializeField] private AudioClip _outOfAmmoClip;
        [SerializeField] private AudioClip _missileClip;

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

            if (tag == "Player")
            {
                _isPlayerShot = true;
                _ammoCount = _maxAmmoCount;

                _gameCanvas = GameObject.Find("Canvas").GetComponent<GameCanvasManager>();
                if (_gameCanvas == null)
                {
                    Debug.LogError("Game Canvas not found on FireProjectiles on " + name);
                }
                _gameCanvas.UpdateAmmoText(_ammoCount, _maxAmmoCount);
            }
            else
            {
                _enemyType = GetComponent<Enemy>().GetEnemyType();
                _isPlayerShot = false;
            }

            if (transform.childCount > 0)
            {
                if (transform.GetChild(0).name == "Alien Shot")
                {
                    _tractorBeam = transform.GetChild(0).gameObject;
                }
            }
        }

        private void OnEnable()
        {
            if (_tractorBeam != null)
            {
                _tractorBeamActive = false;
            }
        }

        public void ShootProjectile()
        {

            if (_enemyType != 2 || _enemyType != 3)
            {
                if (_isTripleShotActive)
                {
                    FireTripleShot();
                    if (_tripleShotCoolDown < Time.time)
                    {
                        _isTripleShotActive = false;
                    }
                }
                else if (_isMissileEnabled)
                {
                    FireMissile();
                }
                else
                {
                    FireLaser();
                }
            }   
            else
            {
                FireLaser();
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

        private void FireTripleShot()
        {
            _tripleShotPool.ShootTripleShot(_isPlayerShot, transform.position);
        }

        public void AmmoPickup()
        {
            _ammoCount = _maxAmmoCount;
            _gameCanvas.UpdateAmmoText(_ammoCount, _maxAmmoCount);
        }

        private void FireLaser()
        {
            if (_isPlayerShot && gameObject.activeInHierarchy)
            {
                if (_ammoCount > 0)
                {
                    _audioSource.clip = _laserSoundClip;
                    _audioSource.Play();
                    _laserShootPosition = new Vector3(transform.position.x, transform.position.y + 1.25f, 0);
                    _ammoCount--;
                    _gameCanvas.UpdateAmmoText(_ammoCount, _maxAmmoCount);
                    _laserPool.ShootLaserFromPool(_isPlayerShot, _laserShootPosition);
                }
                else
                {
                    _gameCanvas.UpdateAmmoText(_ammoCount, _maxAmmoCount);
                    _audioSource.clip = _outOfAmmoClip;
                    _audioSource.Play();
                }
            }
            else
            {                
                switch (_enemyType)
                {
                    case 0:
                        _laserShootPosition = new Vector3(transform.position.x, transform.position.y - 1.3f, 0);
                        _laserPool.ShootLaserFromPool(_isPlayerShot, _laserShootPosition);
                        break;
                    case 1:
                        _laserShootPosition = new Vector3(transform.position.x, transform.position.y - 1f, 0);
                        _laserPool.ShootLaserFromPool(_isPlayerShot, _laserShootPosition);
                        break;
                    case 2:
                        if (!_tractorBeamActive)
                        {
                            _tractorBeam.SetActive(true);
                            _tractorBeamActive = true;
                            StartCoroutine(DisableTractorBeam());
                        }
                        break;
                    case 3:
                        RedFighterShot();
                        break;
                }

                _audioSource.clip = _laserSoundClip;
                _audioSource.Play();
            }
        }

        private void RedFighterShot()
        {
            int randomLaserFire = Random.Range(0, 6);
            List<Vector3> vector3s = new List<Vector3> { new Vector3(transform.position.x + 0.6f, transform.position.y - 5f, 0),
                new Vector3(transform.position.x - 1f, transform.position.y - 5f, 0),
                new Vector3(transform.position.x + 3.65f, transform.position.y - 2f, 0),
                new Vector3(transform.position.x + 3.1f, transform.position.y - 2f, 0),
                new Vector3(transform.position.x - 3.55f, transform.position.y - 2f, 0),
                new Vector3(transform.position.x - 4.1f, transform.position.y - 2f, 0)
            };

            switch (randomLaserFire)
            {
                case 0 or 1:
                    {
                        _laserPool.ShootLaserFromPool(_isPlayerShot, vector3s[0]);
                        _laserPool.ShootLaserFromPool(_isPlayerShot, vector3s[1]);
                        break;
                    }
                    case 2 or 3:
                    {

                        _laserPool.ShootLaserFromPool(_isPlayerShot, vector3s[2]);
                        _laserPool.ShootLaserFromPool(_isPlayerShot, vector3s[3]);
                        break;
                    }
                    case 4 or 5:
                    {
                        _laserPool.ShootLaserFromPool(_isPlayerShot, vector3s[4]);
                        _laserPool.ShootLaserFromPool(_isPlayerShot, vector3s[5]);
                        break;
                    }
            }
            _audioSource.clip = _laserSoundClip;
            _audioSource.Play();
        }

        private IEnumerator DisableTractorBeam()
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 5f));
            _tractorBeam.SetActive(false);
            yield return new WaitForSeconds(Random.Range(0.5f, 5f));
            _tractorBeamActive = false;
        }

        private void FireMissile()
        {
            if (_isPlayerShot)
            {
                _laserShootPosition = new Vector3(transform.position.x, transform.position.y + 1.85f, 0);
            }
            else
            {
                _laserShootPosition = new Vector3(transform.position.x, transform.position.y - 1f, 0);
            }
            _audioSource.clip = _missileClip;
            _audioSource.volume = 1f;
            _audioSource.Play();
            _audioSource.volume = 0.1f;
            _isMissileEnabled = false;
            _missilePool.ShootMissileFromPool(_isPlayerShot, _laserShootPosition);
        }
    }
}

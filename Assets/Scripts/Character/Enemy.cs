using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Utility;

public class Enemy : MonoBehaviour
{
    private static int _enemyCount;
    private float _movementWait = 3f;
    private float _originalMoveSpeed;
    private float _randomXTranslate;
    private float _yTranslate;
    private float _nextFire = -1;
    private float _alienAmplitude = 1f;
    private float _alienFrequency = 0.5f;
    private bool _changeDirection = true;
    private bool _canMove = true;
    [Range(0, 10)][SerializeField] private float _laserWaitTimeMin = 0.5f;
    [Range(0, 10)][SerializeField] private float _laserWaitTimeMax = 3f;
    [Range(0, 10)][SerializeField] private float _moveSpeed = 3f;
    [Range(0, 5f)][SerializeField] private float _speedMultiplier = 1.5f;
    [SerializeField] private bool _canShoot = true;

    private enum EnemyTypes
    {
        Regular,
        RandomEnemy,
        TieFighter,
        Alien,
        Droid,
        RedFighter
    };
    [SerializeField] private EnemyTypes CurrentEnemyType;

    private void OnEnable()
    {
        if (CurrentEnemyType.Equals(EnemyTypes.Alien))
        {
            transform.position = new Vector3(Random.Range(-Helper.GetXPositionBounds(), Helper.GetXPositionBounds()), 3, 0);
            _randomXTranslate = Random.Range(-1f, 1f);
        }
        else if (CurrentEnemyType.Equals(EnemyTypes.RedFighter))
        {
            transform.position = new Vector3(Random.Range(-6f, 6f), 10, 0);
            _randomXTranslate = 1f;
        }
        else
        {
            transform.position = new Vector3(Random.Range(-Helper.GetXPositionBounds(), Helper.GetXPositionBounds()), Helper.GetYUpperScreenBounds(), 0);
            _randomXTranslate = Random.Range(-1f, 1f);
            _yTranslate = Random.Range(0.1f, 1f);
        }

        _nextFire = Time.time + Random.Range(_laserWaitTimeMin, _laserWaitTimeMax);
        _enemyCount++;
        _originalMoveSpeed = _moveSpeed;
    }

    private void OnDisable()
    {
        _enemyCount--;
    }

    void Update()
    {
        Movement();

        if (Time.time > _nextFire && CurrentEnemyType != EnemyTypes.Droid)
        {
            ShootLasers();
        }
    }

    private void Movement()
    {
        Vector3 move;
        switch (CurrentEnemyType)
        {
            case EnemyTypes.Regular:
                {
                    move = new Vector3(0, Random.Range(-0.5f, 0f), 0);
                    transform.Translate(move * _moveSpeed * Time.deltaTime);
                    if (transform.position.x >= Helper.GetXPositionBounds())
                    {
                        _randomXTranslate = Random.Range(-1f, -0.01f);
                    }
                    if (transform.position.x <= -(Helper.GetXPositionBounds()))
                    {
                        _randomXTranslate = Random.Range(0.01f, 1f);
                    }
                    if (transform.position.y < Helper.GetYLowerBounds() - 2)
                    {
                        transform.position = new Vector3(transform.position.x, Helper.GetYUpperScreenBounds() + 1, 0);
                    }
                    break;
                }

            case EnemyTypes.RandomEnemy:
                {
                    move = new Vector3(_randomXTranslate, Random.Range(-0.5f, 0f), 0);
                    transform.Translate(move * _moveSpeed * Time.deltaTime);
                    if (transform.position.x >= Helper.GetXPositionBounds())
                    {
                        _randomXTranslate = Random.Range(-1f, -0.01f);
                    }
                    if (transform.position.x <= -(Helper.GetXPositionBounds()))
                    {
                        _randomXTranslate = Random.Range(0.01f, 1f);
                    }
                    if (transform.position.y < Helper.GetYLowerBounds() - 2)
                    {
                        transform.position = new Vector3(transform.position.x, Helper.GetYUpperScreenBounds() + 1, 0);
                    }
                    if (_changeDirection)
                    {
                        _changeDirection = false;
                        StartCoroutine(RandomMove());
                    }
                    break;
                }

            case EnemyTypes.TieFighter:
                {
                    move = new Vector3(_randomXTranslate, _yTranslate, 0);
                    transform.Translate(move * _moveSpeed * Time.deltaTime);
                    if (transform.position.x >= Helper.GetXPositionBounds())
                    {
                        _randomXTranslate = Random.Range(-1f, -0.01f);
                    }
                    if (transform.position.x <= -(Helper.GetXPositionBounds()))
                    {
                        _randomXTranslate = Random.Range(0.01f, 1f);
                    }
                    if (transform.position.y < Helper.GetYLowerBounds() - 2)
                    {
                        _yTranslate = Random.Range(0.1f, 1f);
                    }
                    if (transform.position.y > Helper.GetYUpperScreenBounds() + 2)
                    {
                        _yTranslate = Random.Range(-0.1f, -1f);
                    }
                    break;
                }

            case EnemyTypes.Alien:
                {
                    float x = (_randomXTranslate * _moveSpeed), y = (Mathf.Sin(Time.time * _alienFrequency) * _alienAmplitude);
                    move = new Vector3(x, y, 0);

                    transform.Translate(move * Time.deltaTime);
                    if (transform.position.x >= Helper.GetXPositionBounds() + 2.5)
                    {
                        _randomXTranslate = Random.Range(-1f, -0.01f);
                        transform.position = new Vector3(transform.position.x, transform.position.y - 1, 0);
                    }
                    if (transform.position.x <= -(Helper.GetXPositionBounds()) - 2.5)
                    {
                        _randomXTranslate = Random.Range(0.01f, 1f);
                        transform.position = new Vector3(transform.position.x, transform.position.y - 1, 0);
                    }
                    if (transform.position.y < Helper.GetYLowerBounds() - 2)
                    {
                        transform.position = new Vector3(transform.position.x, Helper.GetYUpperScreenBounds() + 1, 0);
                    }
                    else if (transform.position.y > Helper.GetYUpperScreenBounds() + 2)
                    {
                        transform.position = new Vector3(transform.position.x, Helper.GetYLowerBounds() - 1, 0);
                    }

                    if (_changeDirection)
                    {
                        _changeDirection = false;
                        StartCoroutine(RandomMove());
                    }
                    break;
                }
            case EnemyTypes.Droid:
                {
                    if (_canMove)
                    {
                        move = new Vector3(Random.Range(0f, 1f), _randomXTranslate, 0);
                        transform.Translate(move * _moveSpeed * Time.deltaTime);
                        if (transform.position.x >= Helper.GetXPositionBounds())
                        {
                            _randomXTranslate = Random.Range(-1f, -0.01f);
                        }
                        if (transform.position.x <= -(Helper.GetXPositionBounds()))
                        {
                            _randomXTranslate = Random.Range(0.01f, 1f);
                        }
                        if (transform.position.y < Helper.GetYLowerBounds() - 2)
                        {
                            transform.position = new Vector3(transform.position.x, Helper.GetYUpperScreenBounds() + 1, 0);
                        }
                        if (_changeDirection)
                        {
                            _changeDirection = false;
                            StartCoroutine(PauseMovement());
                        }
                    }
                    break;
                }

            case EnemyTypes.RedFighter:
                {
                    if (transform.position.x >= 6)
                    {
                        transform.Translate(Vector3.left * _moveSpeed * Time.deltaTime);
                        _randomXTranslate *= -1;
                    }
                    else if (transform.position.x <= -6)
                    {
                        transform.Translate(Vector3.right * _moveSpeed * Time.deltaTime);
                        _randomXTranslate *= -1;
                    }

                    if (transform.position.y >= 6)
                    {
                        move = Vector3.down;
                    }
                    else
                    {
                        move = new Vector3(_randomXTranslate, 0, 0);
                    }
                    transform.Translate(move * _moveSpeed * Time.deltaTime);
                    break;
                }
        }
    }

    public void SpeedPowerup()
    {
        StartCoroutine(SpeedCooldownSequence());
    }

    private IEnumerator SpeedCooldownSequence()
    {
        _moveSpeed *= _speedMultiplier;
        yield return new WaitForSeconds(5f);
        _moveSpeed = _originalMoveSpeed;
    }

    public int GetEnemyType()
    {
        return (int)CurrentEnemyType;
    }

    private IEnumerator PauseMovement()
    {
        float originalMoveSpeed = _moveSpeed;
        yield return new WaitForSeconds(_movementWait);
        _moveSpeed = 0.5f;
        yield return new WaitForSeconds(_movementWait);
        _moveSpeed = originalMoveSpeed;
        _randomXTranslate = Random.Range(-1f, 1f);
        _changeDirection = true;
    }

    private IEnumerator RandomMove()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, _movementWait));
        _moveSpeed = Random.Range(3f, 6f);
        _randomXTranslate = Random.Range(-1f, 1f);
        _changeDirection = true;
    }

    private void ShootLasers()
    {
        if (_canShoot)
        {
            _nextFire = Time.time + Random.Range(_laserWaitTimeMin, _laserWaitTimeMax);
            GetComponent<ProjectileFire.FireProjectiles>().ShootProjectile();
        }
    }

    public void StopMovement()
    {
        _canMove = false;
    }

    public static int GetEnemyCount()
    {
        return _enemyCount;
    }

    public float GetEnemySpeed()
    {
        return _moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && CurrentEnemyType != EnemyTypes.Alien && CurrentEnemyType != EnemyTypes.Droid)
        {
            collision.GetComponent<Health.Health>().DamageTaken(100, false);
            this.GetComponent<Health.Health>().DamageTaken(100, true);
        }
        else if (collision.CompareTag("Enemy") && collision.name != "RedFighter(Clone)" && collision.name != "Droid Enemy(Clone)")
        {
            collision.GetComponent<Health.Health>().DamageTaken(5, false);
            this.GetComponent<Health.Health>().DamageTaken(5, false);
        }
        else if (CurrentEnemyType == EnemyTypes.Droid)
        {
            if (TryGetComponent(out PolygonCollider2D polygonCollider2D) && polygonCollider2D.IsTouching(collision))
            {
                if (collision.CompareTag("Player"))
                {
                    collision.GetComponent<Health.Health>().DamageTaken(50, false);
                    GetComponent<Health.Health>().DamageTaken(50, true);
                }
            }
        }
    }
}

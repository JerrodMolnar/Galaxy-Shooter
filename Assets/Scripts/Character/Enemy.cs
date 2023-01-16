using System.Collections;
using UnityEngine;
using Utility;

public class Enemy : MonoBehaviour
{
    private float _movementWait = 3f;
    private bool _changeDirection = true;
    private float _randomXTranslate;
    private float _nextFire = -1;
    private static int _enemyCount;
    [SerializeField] private float _laserWaitTimeMin = 0.5f;
    [SerializeField] private float _laserWaitTimeMax = 3f;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _amplitude = 2f;
    [SerializeField] private float _frequency = 0.1f;

    private enum EnemyTypes
    {
        Regular,
        RandomEnemy,
        Alien,
        Boss
    };
    [SerializeField] private EnemyTypes CurrentEnemyType;

    private void OnEnable()
    {
        if (CurrentEnemyType.Equals(EnemyTypes.Alien))
        {
            transform.position = new Vector3(Random.Range(-Helper.GetXPositionBounds(), Helper.GetXPositionBounds()), 3, 0);
        }
        else
        {
            transform.position = new Vector3(Random.Range(-Helper.GetXPositionBounds(), Helper.GetXPositionBounds()), Helper.GetYUpperScreenBounds(), 0);
        }
        
        _randomXTranslate = Random.Range(-1f, 1f);
        _nextFire = Time.time + Random.Range(_laserWaitTimeMin, _laserWaitTimeMax);
        _enemyCount++;
    }

    private void OnDisable()
    {
        _enemyCount--;
    }

    void Update()
    {
        Movement();

        if (Time.time > _nextFire)
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
                        _randomXTranslate = Random.Range(-1f, 0.01f);
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
                        _randomXTranslate = Random.Range(-1f, 0.01f);
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

            case EnemyTypes.Alien:
                {
                    float x = (_randomXTranslate * _moveSpeed), y = (Mathf.Sin(Time.time * _frequency) * _amplitude);
                    move = new Vector3(x, y, 0);

                    transform.Translate(move * Time.deltaTime);
                    if (transform.position.x >= Helper.GetXPositionBounds() + 2.5)
                    {
                        _randomXTranslate = Random.Range(-1f, 0.01f);
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
                    break;
                }

            case EnemyTypes.Boss:
                {
                    move = new Vector3(_randomXTranslate, 0, 0);
                    transform.Translate(move * _moveSpeed * Time.deltaTime);
                    if (transform.position.x >= Helper.GetXPositionBounds() || transform.position.x <= -(Helper.GetXPositionBounds()))
                    {
                        _randomXTranslate *= -1;
                    }

                    if (transform.position.y < Helper.GetYLowerBounds() - 2)
                    {
                        transform.position = new Vector3(transform.position.x, Helper.GetYUpperScreenBounds() + 1, 0);
                    }
                    break;
                }
        }
    }

    public int GetEnemyType()
    {
        return (int)CurrentEnemyType;
    }

    private IEnumerator RandomMove()
    {
        yield return new WaitForSeconds(_movementWait);
        _moveSpeed = Random.Range(3f, 6f);
        _randomXTranslate = Random.Range(-1f, 1f);
        _changeDirection = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && CurrentEnemyType != EnemyTypes.Alien)
        {
            other.GetComponent<Health.Health>().DamageTaken(100);
            this.GetComponent<Health.Health>().DamageTaken(100);
        }
    }

    private void ShootLasers()
    {
        _nextFire = Time.time + Random.Range(_laserWaitTimeMin, _laserWaitTimeMax);
        GetComponent<ProjectileFire.FireProjectiles>().ShootProjectile();
    }

    public static int GetEnemyCount()
    {
        return _enemyCount;
    }
}

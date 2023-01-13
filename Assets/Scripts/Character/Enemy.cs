using System.Collections;
using UnityEngine;
using Utility;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 3f;
    private float _movementWait = 3f;
    private bool _changeDirection = true;
    private float _randomXTranslate;
    private float _nextFire = -1;
    [SerializeField] private float _laserWaitTimeMin = 0.5f;
    [SerializeField] private float _laserWaitTimeMax = 3f;

    private enum EnemyTypes
    {
        Regular,
        RandomEnemy,
        Boss
    };
    [SerializeField] private EnemyTypes CurrentEnemyType;


    private void OnEnable()
    {
        transform.position = new Vector3(Random.Range(-Helper.GetXPositionBounds(), Helper.GetXPositionBounds()), Helper.GetYUpperScreenBounds(), 0);
        _randomXTranslate = Random.Range(-1f, 1f);
        _nextFire = Time.time + Random.Range(_laserWaitTimeMin, _laserWaitTimeMax);
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
                        StartCoroutine(MovementCooldown());
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

    private IEnumerator MovementCooldown()
    {
        yield return new WaitForSeconds(_movementWait);
        _moveSpeed = Random.Range(3f, 6f);
        _randomXTranslate = Random.Range(-1f, 1f);
        _changeDirection = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
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
}

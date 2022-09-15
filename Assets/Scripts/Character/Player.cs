using UnityEngine;
using Utility;

public class Player : MonoBehaviour
{

    [SerializeField] private float _moveSpeed = 4.0f;
    private const float _LASER_WAIT_TIME = 0.25f;
    private float _nextFire = 0.0f;
    private float _speedPowerupTime = -1;
    private const float _SPEED_POWERUP_WAIT_TIME = 7.5f;

    void Start()
    {
        transform.position = new Vector3(0, -2.5f, 0);
    }

    void Update()
    {
        if (Time.time > _speedPowerupTime)
        {
            _moveSpeed = 4f;
        }
        Movement();
        FireLaser();
    }

    private void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(moveDirection * _moveSpeed * Time.deltaTime);

        if (transform.position.x > Helper.GetXPositionBounds() + 2f)
        {
            transform.position = new Vector3(-(Helper.GetXPositionBounds() + 2f), transform.position.y, 0);
        }
        else if (transform.position.x < -(Helper.GetXPositionBounds() + 2f))
        {
            transform.position = new Vector3(Helper.GetXPositionBounds() + 2f, transform.position.y, 0);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, Helper.GetYLowerBounds(), 1), 0);
    }

    public void SpeedPowerup()
    {
        _speedPowerupTime = Time.time + _SPEED_POWERUP_WAIT_TIME;
        _moveSpeed *= 2;
    }

    private void FireLaser()
    {
        if (Input.GetAxis("FireLasers") > 0 && Time.time > _nextFire)
        {
            _nextFire = Time.time + _LASER_WAIT_TIME;
            GetComponent<ProjectileFire.FireProjectiles>().ShootProjectile(0);
        }
    }
}

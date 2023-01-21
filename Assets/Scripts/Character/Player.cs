using GameCanvas;
using UnityEngine;
using Utility;

public class Player : MonoBehaviour
{

    private const float _LASER_WAIT_TIME = 0.25f;
    private float _nextFire = 0.0f;
    private float _thrustMaxTime = 100f;
    private bool _isThrustersActive = false;
    private GameCanvasManager _gameCanvasManager;
    private Thruster _thruster;
    private Animator _animator;
    [Range(0, 10f)] [SerializeField] private float _moveSpeed = 4.0f;
    [Range(0, 100f)] [SerializeField] private float _thrusterTime = 100.0f;
    [Range(0, 50f)] [SerializeField] private float _thrustUsageDecreasedTime = 15f;

    void Start()
    {
        transform.position = new Vector3(0, -2.5f, 0);
        _thruster = transform.GetChild(1).GetComponent<Thruster>();
        {
            if (_thruster == null)
            {
                Debug.LogError("Thruster not found on Player Script on " + name);
            }
        }
        _animator = gameObject.GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator not found on Player Script on " + name);
        }

        _gameCanvasManager = GameObject.Find("Canvas").GetComponent<GameCanvasManager>();
        if (_gameCanvasManager == null)
        {
            Debug.LogError("Game Canvas Manager not found on Player Script on " + name);
        }
        else
        {
            _gameCanvasManager.UpdateThrusterBar(_thrusterTime);
        }
    }

    void Update()
    {
        if (Input.GetAxis("Thrusters") > 0)
        {
            ActivateThrusters();
        }
        else
        {
            DeactivateThrusters();
        }

        Movement();
        FireLaser();
    }

    private void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0);

        if (horizontalInput == 0)
        {
            _animator.SetBool("TurnRight", false);
            _animator.SetBool("TurnLeft", false);
        }
        else if (horizontalInput > 0)
        {
            _animator.SetBool("TurnRight", true);
            _animator.SetBool("TurnLeft", false);
        }
        else if (horizontalInput < 0)
        {
            _animator.SetBool("TurnLeft", true);
            _animator.SetBool("TurnRight", false);
        }

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
        _thrusterTime = _thrustMaxTime;
        _gameCanvasManager.UpdateThrusterBar(_thrusterTime);
    }

    private void FireLaser()
    {
        if (Input.GetAxis("FireLasers") > 0 && Time.time > _nextFire)
        {
            _nextFire = Time.time + _LASER_WAIT_TIME;
            GetComponent<ProjectileFire.FireProjectiles>().ShootProjectile();
        }
    }

    private void ActivateThrusters()
    {
        if (_thrusterTime > 0.0f)
        {
            _thrusterTime = _thrusterTime - (Time.deltaTime * _thrustUsageDecreasedTime);
            _gameCanvasManager.UpdateThrusterBar(_thrusterTime);
            if (!_isThrustersActive)
            {
                _isThrustersActive = true;
                _moveSpeed *= 2;
                _thruster.SpeedBoostThrust();
            }
        }
        else if (_thrusterTime <= 0.0f)
        {
            DeactivateThrusters();
        }
    }

    private void DeactivateThrusters()
    {
        if (_isThrustersActive)
        {
            _moveSpeed /= 2;
            _thruster.NormalSpeedThrust();
            _isThrustersActive = false;
        }
    }
}

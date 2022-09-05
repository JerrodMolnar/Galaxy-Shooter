using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class Player : MonoBehaviour
{

    [SerializeField] private float _moveSpeed = 4.0f; //used as player movement speed

    private const float _LASER_WAIT_TIME = 0.25f;

    private float _nextFire = 0.0f; //timer for next fire

    // Start is called before the first frame update
    void Start()
    {
        //take current pos = new pos (0,0,0)
        transform.position = new Vector3(0, -2.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        FireLaser();
    }


    private void Movement()
    {
        //get movement direction using Unity built-in input manager
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //vector3 for movement direction
        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0);

        //make player move using moveDirection * speed at which you want player to move * time interval of seconds each frame
        transform.Translate(moveDirection * _moveSpeed * Time.deltaTime);

        //if player x position goes off screen return to opposite side of screen
        if (transform.position.x > Helper.GetXPositionBounds() + 2f)
        {
            transform.position = new Vector3(-(Helper.GetXPositionBounds() + 2f), transform.position.y, 0);
        }
        else if (transform.position.x < -(Helper.GetXPositionBounds() + 2f))
        {
            transform.position = new Vector3(Helper.GetXPositionBounds() + 2f, transform.position.y, 0);
        }

        //use Mathf.clamp for y position to keep player on screen
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, Helper.GetYLowerBounds(), 0), 0);
    }

    private void FireLaser()
    {
        //if space key hit fire laser
        if (Input.GetAxis("FireLasers") > 0 && Time.time > _nextFire)
        {
            _nextFire = Time.time + _LASER_WAIT_TIME;            
            //shoot lasers
            GetComponent<ProjectileFire.FireProjectiles>().ShootProjectile(0);
        }
    }

}

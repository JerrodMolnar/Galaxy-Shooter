using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    [SerializeField] private int _moveSpeed = 3; //used as player movement speed
    [SerializeField] private float _xScreenBounds = 10.5f;
    [SerializeField] private float _yScreenBounds = -4.5f;

    // Start is called before the first frame update
    void Start()
    {
        //take current pos = new pos (0,0,0)
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
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
        if (transform.position.x > _xScreenBounds)
        {
            transform.position = new Vector3(-_xScreenBounds, transform.position.y, 0);
        }
        else if (transform.position.x < -_xScreenBounds)
        {
            transform.position = new Vector3(_xScreenBounds, transform.position.y, 0);
        }

        //if player y position is greater than 0 then keep y position at 0
        //else if y position is less than bottom of screen keep y position on screen
        if (transform.position.y > 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y < _yScreenBounds)
        {
            transform.position = new Vector3(transform.position.x, _yScreenBounds, 0);
        }

    }
}

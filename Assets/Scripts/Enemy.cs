using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Helper _helper;
    private float _moveSpeed = 3f;
    private float _movementWait = 3f;
    private bool _canMove = true;
    private int randomDirection;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Random.Range(-1f, 1f), _helper.yUpperScreenBounds, 0);        
    }

    // Update is called once per frame
    void Update()
    {
        if (_canMove)
        {
            _canMove = false;
            Movement();
            StartCoroutine(MovementCooldown());            
        }        
    }

    private void Movement()
    {
        if (transform.position.x > _helper.xPositionBounds)
        {
            transform.position = new Vector3(_helper.xPositionBounds, transform.position.y - 1, 0);            
            transform.Translate(new Vector3(-1, transform.position.y, 0) * _moveSpeed * Time.deltaTime);
        }
        if (transform.position.x < -(_helper.xPositionBounds))
        {
            transform.position = new Vector3(-(_helper.xPositionBounds), transform.position.y - 1, 0);
            transform.Translate(new Vector3(1, transform.position.y, 0) * _moveSpeed * Time.deltaTime);
        }
        
    }

    private IEnumerator MovementCooldown()
    {
        _moveSpeed = 0;
        yield return new WaitForSeconds(_movementWait);
        _moveSpeed = Random.Range(0f, 5f);
        _canMove = true;
    }
}

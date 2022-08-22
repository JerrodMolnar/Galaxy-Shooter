using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private Helper _helper;
    [SerializeField] private int _moveSpeed = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LaserMovement();
    }

    private void LaserMovement()
    {
        //make laser move straight up screen from point of instantiation
        transform.Translate(Vector3.up * _moveSpeed * Time.deltaTime);
        if (transform.position.y > _helper.yUpperScreenBoundary)
        {
            gameObject.SetActive(false);
        }
    }
}

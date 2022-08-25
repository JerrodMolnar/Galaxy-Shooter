using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private Helper _helper = new Helper();
    [SerializeField] private float _moveSpeed = 8.0f;
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
        
        if (transform.position.y > _helper.yUpperScreenBounds + 2.5f)
        {
            transform.position = Vector3.zero;
            gameObject.SetActive(false);            
        }
        else
        {
            transform.Translate(Vector3.up * _moveSpeed * Time.deltaTime);
        }
    }
}

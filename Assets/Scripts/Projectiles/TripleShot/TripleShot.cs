using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class TripleShot : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 8.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TripleShotMovement();
    }

    private void TripleShotMovement()
    {
        //if laser was fired from player make lase go up else make laser go down
        
            if (transform.position.y > Helper.GetYUpperScreenBounds() + 5f)
            {
            gameObject.GetComponentInChildren<Transform>().gameObject.SetActive(false);

                
                    gameObject.SetActive(false);
                transform.position = Vector3.zero;
            }
            else
            {
                transform.Translate(Vector3.up * _moveSpeed * Time.deltaTime);
            }
        
    }
}

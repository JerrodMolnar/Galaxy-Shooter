using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 8.0f;
    [SerializeField] private bool playerLaser = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        LaserMovement();
    }

    public void isPlayerLaser(bool isPlayerLaser)
    {
        //was laser shot by player
        playerLaser = isPlayerLaser;
    }

    private void LaserMovement()
    {
        //if laser was fired from player make lase go up else make laser go down
        if (playerLaser)
        {
            if (transform.position.y > Helper.GetYUpperScreenBounds() + 2.5f)
            {
                transform.position = Vector3.zero;
                gameObject.SetActive(false);
            }
            else
            {
                transform.Translate(Vector3.up * _moveSpeed * Time.deltaTime);
            }
        }
        else
        {
            if (transform.position.y < Helper.GetYLowerBounds() - 2.5f)
            {
                transform.position = Vector3.zero;
                gameObject.SetActive(false);
            }
            else
            {
                transform.Translate(Vector3.down * _moveSpeed * Time.deltaTime);
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        //Laser hit another collider
        other.GetComponent<Health>().DamageTaken(15);
        gameObject.SetActive(false);
    }
    }

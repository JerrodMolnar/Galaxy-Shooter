using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleShotLaserTrigger : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Health.Health>().DamageTaken(5);
            gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Projectile"))
        {
            collision.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }        
    }
}

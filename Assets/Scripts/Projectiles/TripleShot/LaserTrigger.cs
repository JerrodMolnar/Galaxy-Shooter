using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaserTrigger : MonoBehaviour
{
    private GameObject _scoreText;
    // Start is called before the first frame update
    void Start()
    {
        _scoreText = GameObject.Find("Score");
    }

    // Update is called once per frame
    void Update()
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            //Laser hit another collider
            if (other.gameObject.activeSelf)
            {
                //Enemy hit by player shot 
                if (other.CompareTag("Enemy"))
                {
                    if (_scoreText != null)
                    {
                        GameObject.Find("Laser").GetComponent<Laser>().SetScore(5);
                        other.GetComponent<Health>()?.DamageTaken(5);
                    }
                    
                }
                else if (other.CompareTag("Player") && !_playerLaser)
                {
                    other.GetComponent<Health>()?.DamageTaken(5);
                }
            }
            if (other.tag == "Laser")
            {
                other.gameObject.SetActive(false);
            }
            gameObject.SetActive(false);
        }
    }
}

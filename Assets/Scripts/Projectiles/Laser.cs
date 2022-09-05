using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace ProjectileType
{

    public class Laser : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 8.0f;
        private bool _playerLaser = true;
        private GameObject _scoreText;
        private static int _score = 0;

        // Start is called before the first frame update
        void Start()
        {
            _scoreText = GameObject.Find("Score");

            if (_scoreText == null)
            {
                Debug.LogError("Score text on Laser not found");
            }
            else
            {
                _scoreText.GetComponent<Text>().text = "Score: " + _score;
            }
        }

        // Update is called once per frame
        void Update()
        {
            LaserMovement();
        }

        private void OnEnable()
        {
            _playerLaser = false;
        }

        public void SetShooter(bool playerLaser)
        {
            _playerLaser = playerLaser;
        }

        private void LaserMovement()
        {
            //if laser was fired from player make lase go up else make laser go down
            if (_playerLaser)
            {
                if (transform.position.y > Helper.GetYUpperScreenBounds() + 2.5f)
                {
                    gameObject.SetActive(false);
                    transform.position = Vector3.zero;
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

        public void SetScore(int pointsToAdd)
        {
            _score += pointsToAdd;
            _scoreText.GetComponent<Text>().text = "Score: " + _score;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            //Laser hit another collider
            if (other.gameObject.activeSelf)
            {
                //Enemy hit by player shot 
                if (other.CompareTag("Enemy") && _playerLaser)
                {
                    SetScore(5);
                    other.GetComponent<Health>()?.DamageTaken(5);
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

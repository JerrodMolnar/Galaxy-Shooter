using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utility;

public class Health : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _lives = 3;
    [SerializeField] private const int _maxLives = 5;
    private GameObject _livesText;
    private GameObject _healthText;
    private SpawnManager _spawnManager;
    private float _reappearWaitTime = 2f;
    private bool _isInvincible = false;
    

    private void Start()
    {
        _health = _maxHealth;
        _livesText = GameObject.Find("Lives");
        _healthText = GameObject.Find("Health");
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        if (_livesText == null)
        {
            Debug.LogError("Lives text not found");
        }
        else if (tag == "Player")
        {
            _livesText.GetComponent<Text>().text = "Lives: " + _lives;
        }

        if ( _healthText == null)
        {
            Debug.LogError("Health Text not found");
        }
        else if (tag == "Player")
        {
            _healthText.GetComponent<Text>().text = "Health: " + _health;
        }        

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager not found on Health script");
        }
    }

    public void DamageTaken(int damageAmount)
    {
        if (!_isInvincible)
        {
            _health -= damageAmount;
        }

        if (_health <= 0)
        {
            TakeLife();
        }
        else if (tag == "Player")
        {
            if (_healthText != null)
            {
                _healthText.GetComponent<Text>().text = "Health: " + _health;
            }
        }
    }

    public void DamageHealed(int healAmount)
    {
        if (_health + healAmount > _maxHealth)
        {
            _health = _maxHealth;
        }
        else
        {
            _health += healAmount;
        }
    }

    public void TakeLife()
    {
        _lives -= 1;
        _health = 0;

        if (tag == "Player")
        {
            GetComponent<Player>().enabled = false;
            _healthText.GetComponent<Text>().text = "Health: " + _health;
        }
        else
        {
            GetComponent<Enemy>().enabled = false;
        }
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;        
        StartCoroutine(Reappear());
    }

    public IEnumerator Reappear()
    {
        float invincibleTime = 3f;
        yield return new WaitForSeconds(_reappearWaitTime);
        if (tag == "Player" && _lives > 0)
        {
            transform.position = new Vector3(0, -2.5f, 0);
            GetComponent<MeshRenderer>().enabled = true;
            GetComponent<BoxCollider>().enabled = true;
            GetComponent<Player>().enabled = true;
            _health = _maxHealth;
            if (_healthText != null)
            {
                _healthText.GetComponent<Text>().text = "Health: " + _health;
            }
            if (_livesText != null)
            {
                _livesText.GetComponent<Text>().text = "Lives: " + _lives;
            }
            StartCoroutine(Invincible(invincibleTime));
        }
        else if (_lives > 0)
        {
            transform.position = new Vector3(Random.Range(-(Helper.GetXPositionBounds()), Helper.GetXPositionBounds()), Helper.GetYUpperScreenBounds(), 0);
            GetComponent<MeshRenderer>().enabled = true;
            GetComponent<BoxCollider>().enabled = true;
            GetComponent<Enemy>().enabled = true;
            StartCoroutine(Invincible(invincibleTime));
        }
        else if (tag == "Player")
        {
            if (_healthText != null)
            {
                _healthText.GetComponent<Text>().text = "Health: " + _health;
            }
            if (_livesText != null)
            {
                _livesText.GetComponent<Text>().text = "Lives: " + _lives;
            }
            _spawnManager.Spawn(false);
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
                Destroy(enemy);
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private IEnumerator Invincible(float timeInvincible)
    {
        _isInvincible = true;
        yield return new WaitForSeconds(timeInvincible);
        _isInvincible = false;
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isGameOver = false;
    private SpawnManager.SpawnManager _spawnManager;

    private void Start()
    {
        if (GameObject.Find("Spawn Manager").TryGetComponent(out SpawnManager.SpawnManager spawnManager))
        {
            _spawnManager = spawnManager;
        }
        else
        {
            Debug.LogError("Spawn Manager is not found on GameManager on" + name);
        }

    }

    void Update()
    {
        if (_isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Health.Health.SetScoreTo0();
                SceneManager.LoadScene(1); // reload game scene
            }
        }
    }

    public void SetGameOver(bool gameOver)
    {
        _isGameOver = gameOver;
        _spawnManager.StopSpawn();
    }  

    public bool IsGameOver()
    {
        return _isGameOver;
    }
}

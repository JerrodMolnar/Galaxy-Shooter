using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isGameOver = false;

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
    }
}

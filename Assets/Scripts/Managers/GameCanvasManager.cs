using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameCanvas
{
    public class GameCanvasManager : MonoBehaviour
    {
        private Text _healthText;
        private Text _livesText;
        private Text _scoreText;
        private Image _livesImage;
        [SerializeField] private Sprite[] _livesSprites;
        private Text _gameOverText;
        private bool _isGameOver = false;
        private GameManager _gameManager;
        private GameObject _escapeMenu;

        void Start()
        {
            _healthText = transform.GetChild(0).GetComponent<Text>();
            if (_healthText == null)
            {
                Debug.LogError("Health text not found on " + name);
            }
            _livesText = transform.GetChild(1).GetComponent<Text>();
            if (_livesText == null)
            {
                Debug.LogError("Lives text not found on " + name);
            }
            _scoreText = transform.GetChild(2).GetComponent<Text>();
            if (_scoreText == null)
            {
                Debug.LogError("Score text not found on " + name);
            }
            _livesImage = transform.GetChild(3).GetComponent<Image>();
            if (_livesImage == null)
            {
                Debug.LogError("Lives image not found on " + name);
            }
            _gameOverText = transform.GetChild(4).GetComponent<Text>();
            if (_gameOverText == null)
            {
                Debug.LogError("Game Over text not found on " + name);
            }
            _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
            if (_gameManager == null)
            {
                Debug.LogError("Game Manager not found on GameCanvasManager Script on " + name);
            }

            _escapeMenu = transform.GetChild(5).gameObject;
            if (_escapeMenu == null)
            {
                Debug.LogError("Escape Menu not found on GameCanvasManager Script on " + name);
            }
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseMenu();
            }
        }

        public void UpdateHealth(int health)
        {
            _healthText.text = "Health: " + health;
        }

        public void UpdateScore(int score)
        {
            _scoreText.text = "Score: " + score;
        }

        public void UpdateLives(int lives)
        {
            if (lives <= 3)
            {
                if (_livesImage.gameObject.activeSelf == false)
                {
                    _livesText.gameObject.SetActive(false);
                    _livesImage.gameObject.SetActive(true);
                }
                _livesImage.sprite = _livesSprites[lives];
            }
            else
            {
                if (!_livesText.gameObject.activeSelf)
                {
                    _livesImage.gameObject.SetActive(false);
                    _livesText.gameObject.SetActive(true);
                }
                _livesText.text = "Lives: " + lives;
            }
        }

        public void UpdateGameOver(bool isGameOver)
        {
            _isGameOver = isGameOver;
            if (_isGameOver)
            {
                _gameManager.SetGameOver(_isGameOver);
                _gameOverText.gameObject.SetActive(true);
                StartCoroutine(GameOver());
            }
        }

        private IEnumerator GameOver()
        {
            while(_gameOverText.gameObject.activeSelf)
            {
                _gameOverText.text = "Game Over";
                yield return new WaitForSeconds(0.5f);
                _gameOverText.text = "";
                yield return new WaitForSeconds(0.5f);
            }
        }

        private void PauseMenu()
        {
            if (!_escapeMenu.activeSelf)
            {
                Time.timeScale = 0;
                _escapeMenu.SetActive(true);
            }
            else
            {
                Time.timeScale = 1f;
                _escapeMenu.SetActive(false);
            }
            
        }    

        public void QuitGame()
        {
            Application.Quit();
        }

        public void ReturnToGame()
        {
            _escapeMenu.SetActive(false);
            Time.timeScale = 1.0f;
        }

        public void MainMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}

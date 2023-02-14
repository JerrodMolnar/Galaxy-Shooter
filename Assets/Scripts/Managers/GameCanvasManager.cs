using ProjectileFire;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameCanvas
{
    public class GameCanvasManager : MonoBehaviour
    {
        [SerializeField] private Sprite[] _livesSprites;
        [SerializeField] private Text _ammoText;
        [SerializeField] private Text _gameOverText;
        [SerializeField] private Text _healthText;
        [SerializeField] private Text _livesText;
        [SerializeField] private Text _scoreText;
        private bool _isGameOver = false;
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private GameObject _escapeMenu;
        [SerializeField] private Slider _thrustBar;
        [SerializeField] private Image _livesImage;

        void Start()
        {
            if (_healthText == null)
            {
                Debug.LogError("Health text not found on " + name);
            }

            if (_livesText == null)
            {
                Debug.LogError("Lives text not found on " + name);
            }

            if (_scoreText == null)
            {
                Debug.LogError("Score text not found on " + name);
            }

            if (_livesImage == null)
            {
                Debug.LogError("Lives image not found on " + name);
            }

            if (_gameOverText == null)
            {
                Debug.LogError("Game Over text not found on " + name);
            }

            if (_gameManager == null)
            {
                Debug.LogError("Game Manager not found on GameCanvasManager Script on " + name);
            }

            if (_escapeMenu == null)
            {
                Debug.LogError("Escape Menu not found on GameCanvasManager Script on " + name);
            }
            else
            {
                _escapeMenu.SetActive(false);
            }

            if (_thrustBar == null)
            {
                Debug.LogError("Thrust Bar not found on GameCanvasManager Script on " + name);
            }

            if (_ammoText == null)
            {
                Debug.LogError("Ammo Text not found on GameCanvasManager Script on " + name);
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
                StartCoroutine(ShowTextOnScreen("Game Over!!!", Mathf.Infinity));
                _gameOverText.transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        private IEnumerator ShowTextOnScreen(string textToShow, float timeLimit)
        {
            _gameOverText.gameObject.SetActive(true);
            float lengthOfTimeToShow = Time.time + timeLimit;
            while (lengthOfTimeToShow > Time.time)
            {
                _gameOverText.text = textToShow;
                yield return new WaitForSeconds(0.25f);
                _gameOverText.text = "";
                yield return new WaitForSeconds(0.25f);
            }
            _gameOverText.gameObject.SetActive(true);
        }

        public void PauseMenu()
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

        public void UpdateThrusterBar(float thrustValue)
        {
            _thrustBar.value = thrustValue;
        }

        public void UpdateWave(int wave)
        {
            _gameOverText.transform.GetChild(0).gameObject.SetActive(false);
            StartCoroutine(ShowTextOnScreen("Wave " + wave + " Beginning...", 5f));
        }

        public void EndWave(int wave)
        {
            _gameOverText.transform.GetChild(0).gameObject.SetActive(false);
            StartCoroutine(ShowTextOnScreen("End of Wave " + wave + "...", 3f));
            GameObject.FindGameObjectWithTag("Player").GetComponent<FireProjectiles>().AmmoPickup();
        }

        public void UpdateAmmoText(int ammo, int maxAmmo)
        {
            _ammoText.text = "Ammo: " + ammo + "/" + maxAmmo;
        }
    }
}

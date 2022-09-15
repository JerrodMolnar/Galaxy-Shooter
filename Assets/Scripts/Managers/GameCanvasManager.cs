using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GameCanvas
{
    public class GameCanvasManager : MonoBehaviour
    {
        private static Text _healthText;
        private static Text _livesText;
        private static Text _scoreText;
        private Image _livesImage;
        [SerializeField] private Sprite[] _livesSprites;
        private static Text _gameOverText;

        public static int score { get; set;}

        public static int health { get; set;}

        public static int lives { get; set;}

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
        }

        private void Update()
        {
            _healthText.text = "Health: " + health;
            _scoreText.text = "Score: " + score;
            if (lives <= 3)
            {
                if(_livesImage.gameObject.activeSelf == false)
                {
                    _livesText.gameObject.SetActive(false);
                    _livesImage.gameObject.SetActive(true);
                }
                _livesImage.sprite = _livesSprites[lives];
                if (lives == 0)
                {
                    _gameOverText.gameObject.SetActive(true);
                    StartCoroutine(GameOver());

                }
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

        private IEnumerator GameOver()
        {
            while(_gameOverText.gameObject.activeSelf)
            {
                _gameOverText.enabled = true;
                yield return new WaitForSeconds(0.5f);
                _gameOverText.enabled = false;
            }
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace GameCanvas
{
    public class GameCanvasManager : MonoBehaviour
    {
        private static Text _healthText;
        private static Text _livesText;
        private static Text _scoreText;

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
        }

        private void Update()
        {
            _healthText.text = "Health: " + health;
            _scoreText.text = "Score: " + score;
            _livesText.text = "Lives: " + lives;
        }
    }
}

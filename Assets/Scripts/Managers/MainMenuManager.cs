using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _titleScreen;
    [SerializeField] private GameObject _creditsText;
    [SerializeField] private GameObject _controlsObject;

    private void Start()
    {
        if (_titleScreen == null)
        {
            Debug.LogError("Title Screen Object not found in MainMenuManager on " + name);
        }
        if (_creditsText == null)
        {
            Debug.LogError("Credits Text Object not found in MainMenuManager on " + name);
        }
        if (_controlsObject == null)
        {
            Debug.LogError("Controls Object not found in MainMenuManager on " + name);
        }
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowControls()
    {
        if (!_controlsObject.activeSelf)
        {
            _creditsText.SetActive(false);
            _titleScreen.SetActive(false);
            _controlsObject.SetActive(true);
        }
        else
        {
            _creditsText.SetActive(true);
            _titleScreen.SetActive(true);
            _controlsObject.SetActive(false);
        }
    }
}

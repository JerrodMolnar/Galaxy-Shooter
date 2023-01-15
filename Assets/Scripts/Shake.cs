using UnityEngine;

public class Shake : MonoBehaviour
{
    [SerializeField] private float _shakeMultiplier = 0.25f;
    [SerializeField] private float _shakeTime = 1f;
    [SerializeField] private float _shakeDuration = 1f;
    private bool _shakeEnabled = false;
    private Vector3 originalPosition;

    private void OnEnable()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (_shakeEnabled)
        {
            if (_shakeTime > 0)
            {
                transform.localPosition = originalPosition + Random.insideUnitSphere * _shakeMultiplier;
                _shakeTime -= Time.deltaTime;
            }
            else
            {
                _shakeTime = _shakeDuration;
                transform.localPosition = originalPosition;
                _shakeEnabled= false;
            }
        }
    }

    public void EnableShake()
    {
        _shakeEnabled = true;
    }
}

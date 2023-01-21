using UnityEngine;

public class Shake : MonoBehaviour
{
    private float _originalTime, _originalDuration, _originalMultiplier;
    private bool _shakeEnabled = false;
    private Vector3 originalPosition;
    [Range(0, 10f)][SerializeField] private float _shakeDuration = 1f;
    [Range(0, 10f)] [SerializeField] private float _shakeMultiplier = 0.25f;
    [Range(0, 10f)] [SerializeField] private float _shakeTime = 1f;

    private void Start()
    {
        _originalDuration = _shakeDuration;
        _originalMultiplier = _shakeMultiplier;
        _originalTime = _shakeTime;
    }

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
                _shakeEnabled = false;
                _shakeTime = _originalTime;
                _shakeMultiplier = _originalMultiplier;
                _shakeDuration = _originalDuration;
            }
        }
    }

    public void EnableShake()
    {
        _shakeEnabled = true;
    }

    public void EnableShake(float shakeTime, float shakeDuration, float shakeMultiplier)
    {
        _shakeDuration = shakeDuration;
        _shakeMultiplier = shakeMultiplier;
        _shakeTime = shakeTime;
        _shakeEnabled = true;

    }
}

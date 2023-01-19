using UnityEngine;

public class TractorBeam : MonoBehaviour
{
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        if (_particleSystem == null)
        {
            Debug.LogError("Particle System on TractorBeam is Null");
        }
    }

    private void OnEnable()
    {
        _particleSystem.Play();
    }

    private void OnDisable()
    {
        _particleSystem.Stop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health.Health health = collision.GetComponent<Health.Health>();
        if (health != null)
        {
            health.DamageTaken(25, false);
        }
    }
}

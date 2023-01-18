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
            if (health.name == "Player")
            {
                GameObject.Find("Main Camera").GetComponent<Shake>().EnableShake(1f, 1f, .5f);
            }
            health.DamageTaken(25);
        }
    }
}

using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private float _rotationSpeed = 25.0f;
    private SpawnManager.SpawnManager _spawnManager;
    private Animator _animator;
    private PolygonCollider2D _collider;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager.SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager not found on Asteroid Script on " + name);
        }

        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator not found on Asteroid Script on " + name);
        }

        _collider = GetComponent<PolygonCollider2D>();
        if (_collider == null)
        {
            Debug.LogError("Polygon Collider not found on Asteroid Script on " + name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, 1 * _rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Projectile" || other.tag == "Player")
        {
            other.gameObject.SetActive(false);
            _rotationSpeed = 0;
            _collider.enabled = false;
            _animator.SetTrigger("Explode");

            _spawnManager.Spawn(true);
        }
    }
}

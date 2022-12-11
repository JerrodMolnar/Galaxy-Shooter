using Unity.VisualScripting;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private float _rotationSpeed = 25.0f;
    private SpawnManager.SpawnManager _spawnManager;
    private Animator _animator;
    private PolygonCollider2D _collider;
    [SerializeField] private AudioClip _explosionClip;
    private AudioSource _explosionSource;

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

        _explosionSource = GetComponent<AudioSource>();
        if (_explosionSource == null)
        {
            _explosionSource = this.AddComponent<AudioSource>();
        }
        else
        {
            _explosionSource.playOnAwake = false;
            _explosionSource.clip = _explosionClip;
            _explosionSource.volume = 0.25f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, 1 * _rotationSpeed * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Alpha0)) 
        {
            _rotationSpeed = 0;
            _collider.enabled = false;
            _animator.SetTrigger("Explode");
            _explosionSource.Play();
            _spawnManager.Spawn(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Projectile")
        {
            other.gameObject.SetActive(false);
            _rotationSpeed = 0;
            _collider.enabled = false;
            _animator.SetTrigger("Explode");
            _explosionSource.Play();
            _spawnManager.Spawn(true);
        }
    }
}

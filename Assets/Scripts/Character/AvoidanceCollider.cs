using UnityEngine;

public class AvoidanceCollider : MonoBehaviour
{
    private Transform _droidTransform;
    private float _moveSpeed = 10f;
    private int _randomInt;
    private Vector2 _direction;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.parent.GetChild(0).TryGetComponent(out Transform droidTransform))
        {
            _droidTransform = droidTransform;
        }
        else
        {
            Debug.LogError("Droid transform not found on Aggression Script on " + name);
        }
    }

    private void Update()
    {
        transform.position = _droidTransform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile"))
        {
            _direction = collision.transform.position - _droidTransform.position;
            _randomInt = Random.Range(0, 5);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            Debug.Log("Avoidance Trigger Stay & random int is " + _randomInt);
            if (_randomInt > 1)
            {
                _droidTransform.position = Vector2.MoveTowards(_droidTransform.position, -other.transform.position, 
                    _moveSpeed * Time.deltaTime);
            }
        }
    }
}

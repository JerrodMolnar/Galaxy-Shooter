using UnityEngine;

public class AvoidanceCollider : MonoBehaviour
{
    private Transform _droidTransform;
    private float _moveSpeed = 10f;
    private int _randomInt;

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
            _randomInt = Random.Range(0, 5);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            if (_randomInt > 2)
            {
                _droidTransform.position = Vector2.MoveTowards(_droidTransform.position, -other.transform.position, 
                    _moveSpeed * Time.deltaTime);
            }
        }
    }
}

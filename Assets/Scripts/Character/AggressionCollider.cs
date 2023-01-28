using UnityEngine;

public class AggressionCollider : MonoBehaviour
{
    private Transform _droidTransform;
    private float _moveSpeed;
    private float _randomInt;
    private Enemy _enemyScript;

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
        if (transform.parent.GetChild(0).TryGetComponent(out Enemy droidEnemy))
        {
            _enemyScript = droidEnemy;
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
        if (collision.CompareTag("Player"))
        {
            _moveSpeed = _enemyScript.GetEnemySpeed();
            _randomInt = Random.Range(0, 5);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_randomInt > 2)
            {
                _droidTransform.position = Vector2.MoveTowards(_droidTransform.position, collision.transform.position, 
                    _moveSpeed * Time.deltaTime);
            }
        }
    }
}

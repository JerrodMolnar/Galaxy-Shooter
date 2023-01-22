using UnityEngine;


namespace ProjectileType
{
    public class Mine : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 3f;
        private float _movementTime = 2f;
        private float _moveStop;
        private bool _blowingUp = false;
        private Animator _animator;

        private void Start()
        {
            if (!TryGetComponent<Animator>(out _animator))
            {
                Debug.LogError("Animator on Mine is null on " + name);
            }
        }

        private void OnEnable()
        {
            _blowingUp = false;
            _moveStop = Time.time + _movementTime;
        }

        void Update()
        {
            Movement();
        }

        private void Movement()
        {
            if (_moveStop > Time.time && !_blowingUp)
            {
                transform.Translate(Vector3.down * _moveSpeed * Time.deltaTime);
            }
        }

        public void BlowUpSequence()
        {
            if (!_blowingUp)
            {
                _blowingUp = true;
                _animator.SetTrigger("Explode");
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            int enemyType = 0;
            if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemyType = enemy.GetEnemyType();
            }

            if (enemyType != 3)
            {
                if (!_blowingUp)
                {
                    _blowingUp = true;
                    _animator.SetTrigger("Explode");
                }

                if (_blowingUp)
                {
                    if (collision.gameObject.TryGetComponent<Health.Health>(out Health.Health health))
                    {
                        health.DamageTaken(25, false);
                    }
                }
            }
        }
    }
}

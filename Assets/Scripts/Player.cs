using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private int _moveSpeed = 4; //used as player movement speed
    [SerializeField] private float _xScreenBounds = 11.3f; //used for player x position bounds
    [SerializeField] private float _yScreenBounds = -4f; //used for player y position bounds


    [SerializeField] private GameObject _laserObject;
    [SerializeField] private float _laserWaitTime = 0.5f;
    private bool _fired = false; //projectile fired boolean;
    private List<GameObject> _projectiles = new List<GameObject>(); //projectiles object pool
    [SerializeField] private int _maxProjectilesForPool = 10; //max lasers in object pool
    private GameObject _laserPool;

    // Start is called before the first frame update
    void Start()
    {
        //take current pos = new pos (0,0,0)
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        FireLaser();
    }

    private void Movement()
    {
        //get movement direction using Unity built-in input manager
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //vector3 for movement direction
        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0);

        //make player move using moveDirection * speed at which you want player to move * time interval of seconds each frame
        transform.Translate(moveDirection * _moveSpeed * Time.deltaTime);

        //if player x position goes off screen return to opposite side of screen
        if (transform.position.x > _xScreenBounds)
        {
            transform.position = new Vector3(-_xScreenBounds, transform.position.y, 0);
        }
        else if (transform.position.x < -_xScreenBounds)
        {
            transform.position = new Vector3(_xScreenBounds, transform.position.y, 0);
        }

        //if player y position is greater than 0 then keep y position at 0
        //else if y position is less than bottom of screen keep y position on screen
        /* if (transform.position.y > 0)
         {
             transform.position = new Vector3(transform.position.x, 0, 0);
         }
         else if (transform.position.y < _yScreenBounds)
         {
             transform.position = new Vector3(transform.position.x, _yScreenBounds, 0);
         }*/

        //could use Mathf.clamp for y position
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, _yScreenBounds, 0), 0);
    }

    private void FireLaser()
    {
        //if space key hit fire laser
        if (Input.GetAxis("Fire1") > 0 && !_fired)
        {
            _fired = true;

            if (_laserPool == null)
            {
                _laserPool = new GameObject("Laser Object Pool");
            }
            //shoot limited lasers
            StartCoroutine(LimitProjectiles(_laserObject, _laserWaitTime, _laserPool));
        }
    }

    private IEnumerator LimitProjectiles(GameObject projectile, float waitTime, GameObject pool)
    {
        Vector3 shootPosition = new Vector3(transform.position.x, transform.position.y + 1f, 0);
        // add projectile to pool
        if (_projectiles.Count < _maxProjectilesForPool)
        {
            GameObject newProjectile = Instantiate(projectile, shootPosition, Quaternion.identity, pool.transform);
            _projectiles.Add(newProjectile);
        }
        //use projectile from pool
        else
        {
            foreach (GameObject itemInPool in _projectiles)
            {
                if (!itemInPool.activeSelf)
                {
                    itemInPool.transform.position = shootPosition;
                    itemInPool.SetActive(true);
                    break;
                }
            }
        }

        yield return new WaitForSeconds(waitTime);
        _fired = false;
    }
}

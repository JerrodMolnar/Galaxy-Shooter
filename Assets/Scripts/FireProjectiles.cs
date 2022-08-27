using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectiles : MonoBehaviour
{
    private int _maxProjectilesForPool = 30;
    private static GameObject _laserPoolParent;
    private static List<GameObject> _laserList = new List<GameObject>();
    [SerializeField] GameObject _laser;

    private enum ProjectileType
    {
        Laser, //0
        Rocket //1
    };

    // Start is called before the first frame update
    void Start()
    {
        if (_laserPoolParent == null)
        {
        _laserPoolParent = new GameObject("Laser Object Pool");
        }

        if (_laser == null)
        {
            Debug.LogError("Laser Prefab not found");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShootProjectile(int projectileType)
    {
        switch (projectileType)
        {
            case (int)ProjectileType.Laser:
                ProjectileChoice(_laser, _laserList, _laserPoolParent);
                break;
                
        }
    }

    private void ProjectileChoice(GameObject projectile, List<GameObject> projectiles, GameObject poolParent)
    {
        bool playerLaser = false;
        Vector3 shootPosition;
        if (tag == "Player")
        {
            playerLaser = true;
            shootPosition = new Vector3(transform.position.x, transform.position.y + 1f, 0);
        }
        else
        {
            shootPosition = new Vector3(transform.position.x, transform.position.y - 1f, 0);
        }
        
        
        // if projectiles are not at max amount then add projectile to pool
        if (poolParent != null)
        {
            if (projectiles.Count < _maxProjectilesForPool)
            {
                GameObject newProjectile = Instantiate(projectile, shootPosition, Quaternion.identity, poolParent.transform);
                projectiles.Add(newProjectile);
                if (playerLaser)
                {
                    newProjectile.GetComponent<Laser>().isPlayerLaser(true);
                }
            }
            //else use inactive projectile from pool
            else
            {
                foreach (GameObject itemInPool in projectiles)
                {
                    if (!itemInPool.activeSelf)
                    {
                        itemInPool.transform.position = shootPosition;
                        if (playerLaser)
                        {
                            itemInPool.GetComponent<Laser>().isPlayerLaser(true);
                        }
                        itemInPool.SetActive(true);
                        break;
                    }
                }
            }
        }
        
    }
}

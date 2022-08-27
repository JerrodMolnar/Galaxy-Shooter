using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectiles : MonoBehaviour
{
    private int _maxProjectilesForPool = 30;
    private static GameObject _laserPoolParent;
    private static List<GameObject> _laserList = new List<GameObject>();
    [SerializeField] GameObject _laser;

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
            case 0: //laser
                ProjectileChoice(_laser, _laserList, _laserPoolParent);
                break;
            default:
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


        // if projectiles are not at max amount then add projectile to pool as needed
        if (poolParent != null)
        {
            if (projectiles.Count < 1)
            {
                GameObject newProjectile = Instantiate(projectile, shootPosition, Quaternion.identity, poolParent.transform);
                projectiles.Add(newProjectile);
                if (playerLaser)
                {
                    newProjectile.GetComponent<Laser>().isPlayerLaser(true);
                }
            }
            foreach (GameObject itemInPool in projectiles)
            {
                if (!itemInPool.activeSelf)
                {
                    itemInPool.SetActive(true);
                    itemInPool.transform.position = shootPosition;
                    if (playerLaser)
                    {
                        itemInPool.GetComponent<Laser>().isPlayerLaser(true);
                    }
                    break;
                }
                else if (projectiles.Count < _maxProjectilesForPool)
                {
                    GameObject newProjectile = Instantiate(projectile, shootPosition, Quaternion.identity, poolParent.transform);
                    projectiles.Add(newProjectile);
                    if (playerLaser)
                    {
                        newProjectile.GetComponent<Laser>().isPlayerLaser(true);
                    }
                }
            }
        }
    }
}

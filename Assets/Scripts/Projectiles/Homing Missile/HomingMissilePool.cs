using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissilePool : MonoBehaviour
{
    [SerializeField] private GameObject _homingMissilePrefab;
    private List<GameObject> _homingMissileList = new List<GameObject>();

    void Start()
    {
        if (_homingMissilePrefab == null)
        {
            Debug.LogError("Missile prefab missing on " + name);
        }
    }

    public void ShootMissileFromPool(bool isPlayerMissile, Vector3 shotPosition)
    {
        if (_homingMissilePrefab != null)
        {
            bool isNoActiveMissile = true;

            foreach (GameObject itemInPool in _homingMissileList)
            {
                if (itemInPool.activeSelf == false)
                {
                    itemInPool.SetActive(true);
                    itemInPool.GetComponent<ProjectileType.HomingMissile>().SetShooter(isPlayerMissile);
                    itemInPool.transform.position = shotPosition;
                    isNoActiveMissile = false;
                    break;
                }
            }
            if (isNoActiveMissile)
            {
                GameObject newProjectile = Instantiate(_homingMissilePrefab, shotPosition, Quaternion.identity, transform);
                newProjectile.GetComponent<ProjectileType.HomingMissile>().SetShooter(isPlayerMissile);
                _homingMissileList.Add(newProjectile);
            }
        }
    }
}

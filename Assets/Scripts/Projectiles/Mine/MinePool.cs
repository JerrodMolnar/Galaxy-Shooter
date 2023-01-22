using System.Collections.Generic;
using UnityEngine;

public class MinePool : MonoBehaviour
{
    [SerializeField] private GameObject _minePrefab;
    private List<GameObject> _mineList = new List<GameObject>();

    void Start()
    {
        if (_minePrefab == null)
        {
            Debug.LogError("Mine prefab missing on " + name);
        }
    }

    public void LayMine(Vector3 shotPosition)
    {
        if (_minePrefab != null)
        {
            bool isNoActiveMine = true;

            foreach (GameObject itemInPool in _mineList)
            {
                if (itemInPool.activeSelf == false)
                {
                    itemInPool.SetActive(true);
                    itemInPool.transform.position = shotPosition;
                    isNoActiveMine = false;
                    break;
                }
            }
            if (isNoActiveMine)
            {
                GameObject newProjectile = Instantiate(_minePrefab, shotPosition, Quaternion.identity, transform);
                _mineList.Add(newProjectile);
            }
        }
    }
}

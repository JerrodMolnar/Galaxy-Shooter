using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private GameObject _laser;

    private static PoolManager _instance;
    public static PoolManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("PoolManager not found");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    public List<GameObject> GenerateProjectiles()
    {
        return null;
    }
}

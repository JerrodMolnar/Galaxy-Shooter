using UnityEngine;

public class RotateItem : MonoBehaviour
{
    [Range(0, 100f)] [SerializeField] private float _rotationSpeed = 25f;

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            transform.Rotate(Vector3.up * _rotationSpeed);
        }
    }
}

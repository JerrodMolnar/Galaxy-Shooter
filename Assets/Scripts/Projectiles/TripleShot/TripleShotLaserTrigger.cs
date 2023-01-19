using UnityEngine;

public class TripleShotLaserTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Health.Health>().DamageTaken(5, true);
            gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Projectile"))
        {
            collision.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }        
    }
}

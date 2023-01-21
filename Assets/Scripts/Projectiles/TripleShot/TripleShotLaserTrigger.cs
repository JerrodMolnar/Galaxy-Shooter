using UnityEngine;

public class TripleShotLaserTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool playerShot = GetComponentInParent<ProjectileType.TripleShot>().IsPlayerShot();
        if (collision.CompareTag("Enemy") && playerShot)
        {
            collision.GetComponent<Health.Health>().DamageTaken(5, playerShot);
            gameObject.SetActive(false);
        }
        if (collision.CompareTag("Player") && !playerShot)
        {
            collision.GetComponent<Health.Health>().DamageTaken(5, playerShot);
            gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Projectile"))
        {
            collision.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }        
    }
}

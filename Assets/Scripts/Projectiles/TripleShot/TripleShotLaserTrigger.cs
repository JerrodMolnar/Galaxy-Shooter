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
            collision.GetComponent<Health.Health>().DamageTaken(10, playerShot);
            gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Projectile"))
        {
            if (collision.gameObject.TryGetComponent<ProjectileType.Mine>(out ProjectileType.Mine mine))
            {
                mine.BlowUpSequence();
            }
            else if (collision.gameObject.name != "Missile(Clone)")
            {
                collision.gameObject.SetActive(false);
            }
            gameObject.SetActive(false);
        }        
    }
}

using UnityEngine;
using System.Collections;

public class BulletCollision : MonoBehaviour
{
    public ParticleSystem wallParticles;
    public ParticleSystem barrelParticles;
    public ParticleSystem cactusParticles;
	
    void OnCollisionEnter(Collision collision)
    {
        // bullet hits wall
        if (collision.gameObject.CompareTag("Wall")) {
            PlayParticleEffect(wallParticles);
            Destroy(gameObject);
        }
        
        // player bullet
        if (CompareTag("PlayerBullet")) {
            if (collision.gameObject.CompareTag("Enemy")) {
                Debug.Log("we shot bandit");
                collision.collider.SendMessage("TakeDamage", 1);
                Destroy(gameObject);
            }
        }
        
        // enemy bullet
        if (CompareTag("EnemyBullet")) {
            if (collision.gameObject.CompareTag("Player")) {
                collision.collider.SendMessage("TakeDamage", 1);
                Destroy(gameObject);
            }
        }
        
        // bullet hits barrel
        if (collision.gameObject.CompareTag("Barrel")) {
            PlayParticleEffect(barrelParticles);
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        
        // bullet hits cactus
        if (collision.gameObject.CompareTag("Cactus")) {
            PlayParticleEffect(cactusParticles);
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
	
    void PlayParticleEffect(ParticleSystem particles)
    {
        // always be firing particles back as us
        float y = transform.rotation.eulerAngles.y + 180;
		
        ParticleSystem part = GameObject.Instantiate(particles, transform.position, Quaternion.Euler(20, y, 0)) as ParticleSystem;
        part.Play();
    }
}

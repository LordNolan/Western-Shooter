using UnityEngine;
using System.Collections;

public class BulletCollision : MonoBehaviour
{
    public ParticleSystem wallParticles;
    public ParticleSystem barrelParticles;
    public ParticleSystem cactusParticles;
    public ParticleSystem floorParticles;
    
    public int damage = 1;
    public int damageModifier = 1;
	
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
                collision.collider.SendMessage("TakeDamage", damage * damageModifier);
                Destroy(gameObject);
            }
        }
        
        // enemy bullet
        if (CompareTag("EnemyBullet")) {
            if (!InNonPlayingState() && collision.gameObject.CompareTag("Player")) {
                collision.collider.SendMessage("TakeDamage", damage);
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
        
        // bullet hits ground
        if (collision.gameObject.CompareTag("Floor")) {
            PlayGroundParticleEffect(floorParticles);
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
    
    void PlayGroundParticleEffect(ParticleSystem particles)
    {
        // get impact spot with y of particle effect
        Vector3 position = new Vector3(transform.position.x, particles.transform.position.y, transform.position.z); 
        // get y rotation of bullet and x rotation of particle effect
        Quaternion rotation = Quaternion.Euler(particles.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 180, 0);
        ParticleSystem part = GameObject.Instantiate(particles, position, rotation) as ParticleSystem;
        part.Play();
    }
}

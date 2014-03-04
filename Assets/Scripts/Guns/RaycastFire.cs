using UnityEngine;
using System.Collections;

public class RaycastFire : MonoBehaviour
{
    public ParticleSystem wallParticles;
    public ParticleSystem barrelParticles;
    public ParticleSystem cactusParticles;
    public ParticleSystem floorParticles;
    public ParticleSystem bloodParticles;
    
    public int damage = 1;
    public int pu_DamageModifier = 1;  // variable for double damage
    
    public void Fire(Vector3 origin, Vector3 direction)
    {
        RaycastHit rayHit;
        if (Physics.Raycast(origin, direction, out rayHit)) {
            
            // bullet hits wall
            if (rayHit.collider.gameObject.CompareTag("Wall")) {
                PlayParticleEffect(wallParticles, rayHit.point);
            }
            
            // bullet hit enemy
            if (rayHit.collider.gameObject.CompareTag("Enemy")) {
                rayHit.collider.SendMessage("TakeDamage", damage * pu_DamageModifier);
                PlayParticleEffect(bloodParticles, rayHit.point);
            }
            
            // bullet hits barrel
            if (rayHit.collider.gameObject.CompareTag("Barrel")) {
                PlayParticleEffect(barrelParticles, rayHit.point);
                Destroy(rayHit.collider.gameObject);
            }
            
            // bullet hits cactus
            if (rayHit.collider.gameObject.CompareTag("Cactus")) {
                PlayParticleEffect(cactusParticles, rayHit.point);
                Destroy(rayHit.collider.gameObject);
            }
            
            // bullet hits ground
            if (rayHit.collider.gameObject.CompareTag("Floor")) {
                PlayGroundParticleEffect(floorParticles, rayHit.point);
            }
        }
    }
    
    void PlayParticleEffect(ParticleSystem particles, Vector3 point)
    {
        // always be firing particles back as us
        float y = transform.parent.transform.rotation.eulerAngles.y + 180;
     
        ParticleSystem part = GameObject.Instantiate(particles, point, Quaternion.Euler(20, y, 0)) as ParticleSystem;
        part.Play();
    }
    
    void PlayGroundParticleEffect(ParticleSystem particles, Vector3 point)
    {
        // get impact spot with y of particle effect
        Vector3 position = new Vector3(point.x, particles.transform.position.y, point.z); 
        // get y rotation of bullet and x rotation of particle effect
        Quaternion rotation = Quaternion.Euler(particles.transform.rotation.eulerAngles.x, transform.parent.transform.rotation.eulerAngles.y + 180, 0);
        ParticleSystem part = GameObject.Instantiate(particles, position, rotation) as ParticleSystem;
        part.Play();
    }
}

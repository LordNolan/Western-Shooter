using UnityEngine;
using System.Collections;

public class BulletCollision : MonoBehaviour 
{
	public ParticleSystem wallParticles;
	
	void OnCollisionEnter(Collision collision)
	{
		// bullet hits wall
		if (collision.gameObject.CompareTag("Wall"))
		{
			PlayParticleEffect(wallParticles);
			Destroy(gameObject);
		}
	}
	
	void PlayParticleEffect(ParticleSystem particles)
	{
		// always be firing particles back as us
		float y = transform.rotation.eulerAngles.y + 180;
		
		ParticleSystem part = GameObject.Instantiate(particles, transform.position, Quaternion.Euler(20,y,0)) as ParticleSystem;
        part.Play();
	}
}

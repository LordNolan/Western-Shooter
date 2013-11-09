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
		ParticleSystem part = GameObject.Instantiate(particles, transform.position, transform.rotation) as ParticleSystem;
        part.Play();
	}
}

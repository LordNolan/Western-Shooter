using UnityEngine;
using System.Collections;

public class PistolAnimation : MonoBehaviour
{
    public GameObject hammerBone;
    public GameObject chamberBone;
    public GameObject rigBone;
    public AudioSource chamberRotate;
    public ParticleSystem smokeParticles;
    Quaternion hammerInitRotation;
    Transform smokeTransform;
    
    void Start()
    {
        hammerInitRotation = hammerBone.transform.localRotation;
        smokeTransform = transform.FindChild("Smoke");
    }
    
    public void MuzzleFlash()
    {
        ParticleSystem s = GameObject.Instantiate(smokeParticles, smokeTransform.position, smokeTransform.rotation) as ParticleSystem;
        s.transform.parent = transform;
        s.Play();
    }
    
    public void Fire()
    {
        AnimateHammer();
        AnimateChamber();
        Recoil();
    }
    
    public void FireEmpty()
    {
        AnimateHammer();
        AnimateChamber();
    }
    
    void AnimateHammer()
    {
        // hammer goes instantly full down then lerps back to pulled back
        hammerBone.transform.localRotation = hammerInitRotation;
        iTween.RotateAdd(hammerBone, iTween.Hash("amount", Vector3.up * 40.0f, "easeType", "easeOutCubic", "delay", 0.2f, "time", 0.5f));
    }
    
    void AnimateChamber() // clockwise
    {
        iTween.RotateAdd(chamberBone, iTween.Hash("amount", Vector3.right * 60.0f, "easeType", "easeOutCubic", "delay", 0.2f, "time", 0.5f));
    }
    
    void Recoil()
    {
        iTween.RotateAdd(rigBone, iTween.Hash("amount", Vector3.forward * 30.0f, "easeType", "easeOutElastic", "time", 0.15f, "oncomplete", "SettleRecoil", "oncompletetarget", gameObject));
    }
    
    void SettleRecoil()
    {
        iTween.RotateAdd(rigBone, iTween.Hash("amount", Vector3.back * 30.0f, "easeType", "easeOutSine", "time", 0.15f));
        chamberRotate.Play();
    }
}

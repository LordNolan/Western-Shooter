using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;

public class PistolAnimation : MonoBehaviour {
 
    public Transform hammerBone;
    public Transform chamberBone;
	
    Quaternion hammerInitRotation;
    
    void Start()
    {
        hammerInitRotation = hammerBone.rotation;
    }
    
    float time = 0;
    int shotsFired = 0;
	void Update () {
        //chamberBone.Rotate(Vector3.right * Time.deltaTime * 125.0f);
        //hammerBone.Rotate(Vector3.up * Time.deltaTime * 1.0f);
        if ((time += Time.deltaTime) > 2)
        {
            time = 0;
            AnimateHammer();
            AnimateChamber();
        }
	}
    
    void AnimateHammer()
    {
        // hammer goes instantly full down then lerps back to pulled back
        hammerBone.rotation = hammerInitRotation;
        HOTween.To(hammerBone, 0.5f, new TweenParms().Prop("rotation", Vector3.right * 40.0f, true).Delay(0.2f).Ease(EaseType.EaseOutCubic));
    }
    
    void AnimateChamber()
    {
        // when hammer pulled back, rotate chamber
        HOTween.To(chamberBone, 0.5f, new TweenParms().Prop("rotation", Vector3.right * 60.0f, true).Delay(0.2f).Ease(EaseType.EaseOutCubic));
    }
}

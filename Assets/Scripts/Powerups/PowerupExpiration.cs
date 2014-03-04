using UnityEngine;
using System.Collections;

public class PowerupExpiration : MonoBehaviour
{

    public float aliveTime;
    public float blinkSlowTime;
    public float blinkFastTime;

    public float blinkSlowRate;
    public float blinkFastRate;

    float currentTime = 0;
    float currentBlinkTime = 0;
    bool blinkFast = false;
    bool blinkSlow = false;

    void Start()
    {
        currentTime = aliveTime;
    }

    void Update()
    {
        currentTime -= Time.deltaTime;

        if (currentTime < 0) {
            Destroy(gameObject);
        } else if (currentTime < blinkFastTime) {
            blinkFast = true;
        } else if (currentTime < blinkSlowTime) {
            blinkSlow = true;
        }

        if (blinkFast && ((currentBlinkTime += Time.deltaTime) >= blinkFastRate)) {
            renderer.enabled = !renderer.enabled;
            currentBlinkTime = 0;
        } else if (blinkSlow && ((currentBlinkTime += Time.deltaTime) >= blinkSlowRate)) {
            renderer.enabled = !renderer.enabled;
            currentBlinkTime = 0;
        }
    }
}
